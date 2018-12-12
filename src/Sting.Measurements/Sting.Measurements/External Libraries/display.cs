/**
 *  Character-LCD-over-I2C 
 *  ===================
 *  Connect HD44780 LCD character Display to Windows 10 IoT devices via I2C and PCF8574
 *
 *  Author: Jaroslav Zivny
 *  Modified by: Robin Müller
 *  Version: 1.1
 *  Keywords: Windows IoT, LCD, HD44780, PCF8574, I2C bus, Raspberry Pi 2
 *  Git: https://github.com/DzeryCZ/Character-LCD-over-I2C
**/


using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace Sting.Measurements.External_Libraries
{
    class Display
    {

        private const byte LcdWrite = 0x07;

        private readonly byte _d4;
        private readonly byte _d5;
        private readonly byte _d6;
        private readonly byte _d7;
        private readonly byte _en;
        private readonly byte _rw;
        private readonly byte _rs;
        private readonly byte _bl;

        private readonly byte[] _lineAddress = { 0x00, 0x40 };
        private byte _backLight = 0x01;
        private I2cDevice _i2CPortExpander;


        public Display(byte deviceAddress, string controllerName, byte Rs, byte Rw, byte En, byte D4, byte D5, byte D6, byte D7, byte Bl, byte[] lineAddress) : this(deviceAddress, controllerName, Rs, Rw, En, D4, D5, D6, D7, Bl)
        {
            _lineAddress = lineAddress;
        }


        public Display(byte deviceAddress, string controllerName, byte Rs, byte Rw, byte En, byte D4, byte D5, byte D6, byte D7, byte Bl)
        {
            // Configure pins
            _rs = Rs;
            _rw = Rw;
            _en = En;
            _d4 = D4;
            _d5 = D5;
            _d6 = D6;
            _d7 = D7;
            _bl = Bl;

            Task.Run(() => StartI2C(deviceAddress, controllerName)).Wait();
        }



        /**
        * Start I2C Communication
        **/
        public async void StartI2C(byte deviceAddress, string controllerName)
        {
            try
            {
                var i2CSettings = new I2cConnectionSettings(deviceAddress) { BusSpeed = I2cBusSpeed.FastMode };
                var deviceSelector = I2cDevice.GetDeviceSelector(controllerName);
                var i2CDeviceControllers = await DeviceInformation.FindAllAsync(deviceSelector);
                _i2CPortExpander = await I2cDevice.FromIdAsync(i2CDeviceControllers[0].Id, i2CSettings);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}", e.Message);
            }
        }


        /**
        * Initialization
        **/
        public void Init(bool turnOnDisplay = true, bool turnOnCursor = false, bool blinkCursor = false, bool cursorDirection = true, bool textShift = false)
        {
            /* Init sequence */
            Task.Delay(100).Wait();
            PulseEnable(Convert.ToByte((1 << _d5) | (1 << _d4)));
            Task.Delay(5).Wait();
            PulseEnable(Convert.ToByte((1 << _d5) | (1 << _d4)));
            Task.Delay(5).Wait();
            PulseEnable(Convert.ToByte((1 << _d5) | (1 << _d4)));

            /*  Init 4-bit mode */
            PulseEnable(Convert.ToByte((1 << _d5)));

            /* Init 4-bit mode + 2 line */
            PulseEnable(Convert.ToByte((1 << _d5)));
            PulseEnable(Convert.ToByte((1 << _d7)));

            /* Turn on Display, cursor */
            PulseEnable(0);
            PulseEnable(Convert.ToByte((1 << _d7) | (Convert.ToByte(turnOnDisplay) << _d6) | (Convert.ToByte(turnOnCursor) << _d5) | (Convert.ToByte(blinkCursor) << _d4)));

            ClrScr();

            PulseEnable(0);
            PulseEnable(Convert.ToByte((1 << _d6) | (Convert.ToByte(cursorDirection) << _d5) | (Convert.ToByte(textShift) << _d4)));
        }


        /**
        * Turn the backlight ON.
        **/
        public void TurnOnBacklight()
        {
            _backLight = 0x01;
            SendCommand(0x00);
        }


        /**
        * Turn the backlight OFF.
        **/
        public void TurnOffBacklight()
        {
            _backLight = 0x00;
            SendCommand(0x00);
        }

        /// <summary>
        /// Indicates the current status of the Backlight.
        /// </summary>
        /// <returns>Returns true if the backlight is currently on. Returns false otherwise.</returns>
        public bool BacklightStatus()
        {
            return _backLight == 0x01;
        }


        /**
        * Can print string onto Display
        **/
        public void Prints(string text)
        {
            foreach (var t in text)
            {
                PrintC(t);
            }
        }


        /**
        * Print single character onto Display
        **/
        public void PrintC(char letter)
        {
            try
            {
                Write(Convert.ToByte(letter), 1);
            }
            catch (Exception e)
            {

            }
        }


        /**
        * skip to second line
        **/
        public void GotoSecondLine()
        {
            SendCommand(0xc0);
        }


        /**
        * goto X and Y 
        **/
        public void GoToXy(int x, int y)
        {
            SendCommand(Convert.ToByte(x | _lineAddress[y] | (1 << LcdWrite)));
        }


        /**
        * Send data to Display
        **/
        public void SendData(byte data)
        {
            Write(data, 1);
        }


        /**
        * Send command to Display
        **/
        public void SendCommand(byte data)
        {
            Write(data, 0);
        }


        /**
        * Clear Display and set cursor at start of the first line
        **/
        public void ClrScr()
        {
            PulseEnable(0);
            PulseEnable(Convert.ToByte((1 << _d4)));
            Task.Delay(5).Wait();
        }


        /**
        * Send pure data to Display
        **/
        public void Write(byte data, byte Rs)
        {
            PulseEnable(Convert.ToByte((data & 0xf0) | (Rs << _rs)));
            PulseEnable(Convert.ToByte((data & 0x0f) << 4 | (Rs << _rs)));
        }


        /**
        * Create falling edge of "enable" pin to write data/inctruction to Display
        */
        private void PulseEnable(byte data)
        {
            _i2CPortExpander.Write(new[] { Convert.ToByte(data | (1 << _en) | (_backLight << _bl)) }); // Enable bit HIGH
            _i2CPortExpander.Write(new[] { Convert.ToByte(data | (_backLight << _bl)) }); // Enable bit LOW
        }


        /**
        * Save custom symbol to CGRAM
        **/
        public void CreateSymbol(byte[] data, byte address)
        {
            SendCommand(Convert.ToByte(0x40 | (address << 3)));
            foreach (var t in data)
            {
                SendData(t);
            }
        }


        /**
        * Print custom symbol
        **/
        public void PrintSymbol(byte address)
        {
            SendData(address);
        }

        /// <summary>
        /// Handle release of resources
        /// </summary>
        public void Dispose()
        {
            _i2CPortExpander.Dispose();
        }
    }
}