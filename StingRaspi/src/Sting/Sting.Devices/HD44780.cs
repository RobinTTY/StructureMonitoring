using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;


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

namespace Sting.Devices
{
    class Hd44780 //: IGpioComponent
    {
        // Device
        private const string I2CControllerName = "I2C1";
        private const byte DeviceI2CAddress = 0x27;
        private I2cDevice _i2CPortExpander;

        private readonly byte[] _lineAddress = { 0x00, 0x40 };
        private const byte LcdWrite = 0x07;
        private byte _backLight = 0x01;
        
        // Setup pins
        private const byte D4 = 0x04;
        private const byte D5 = 0x05;
        private const byte D6 = 0x06;
        private const byte D7 = 0x07;
        private const byte En = 0x02;
        private const byte Rw = 0x01;
        private const byte Rs = 0x00;
        private const byte Bl = 0x03;


        /// <inheritdoc />
        public bool InitComponent(int pin = 0)
        {
            StartI2C(DeviceI2CAddress, I2CControllerName);
            try
            {
                Init();
            }
            catch (FileNotFoundException e)
            {
                Dispose();
                Debug.WriteLine("Lcd couldn't be initialized, check connections");
            }

            return true;
        }

        /// <inheritdoc />
        public void ClosePin()
        {
            ClrScr();
            Dispose();
        }

        /// <summary>
        /// Start I2C Communication
        /// </summary>
        /// <param name="deviceAddress"></param>
        /// <param name="controllerName"></param>
        private async void StartI2C(byte deviceAddress, string controllerName)
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
            PulseEnable(Convert.ToByte((1 << D5) | (1 << D4)));
            Task.Delay(5).Wait();
            PulseEnable(Convert.ToByte((1 << D5) | (1 << D4)));
            Task.Delay(5).Wait();
            PulseEnable(Convert.ToByte((1 << D5) | (1 << D4)));

            /*  Init 4-bit mode */
            PulseEnable(Convert.ToByte((1 << D5)));

            /* Init 4-bit mode + 2 line */
            PulseEnable(Convert.ToByte((1 << D5)));
            PulseEnable(Convert.ToByte((1 << D7)));

            /* Turn on Display, cursor */
            PulseEnable(0);
            PulseEnable(Convert.ToByte((1 << D7) | (Convert.ToByte(turnOnDisplay) << D6) | (Convert.ToByte(turnOnCursor) << D5) | (Convert.ToByte(blinkCursor) << D4)));

            ClrScr();

            PulseEnable(0);
            PulseEnable(Convert.ToByte((1 << D6) | (Convert.ToByte(cursorDirection) << D5) | (Convert.ToByte(textShift) << D4)));
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
        private void SendData(byte data)
        {
            Write(data, 1);
        }


        /**
        * Send command to Display
        **/
        private void SendCommand(byte data)
        {
            Write(data, 0);
        }


        /**
        * Clear Display and set cursor at start of the first line
        **/
        private void ClrScr()
        {
            PulseEnable(0);
            PulseEnable(Convert.ToByte((1 << D4)));
            Task.Delay(5).Wait();
        }


        /**
        * Send pure data to Display
        **/
        private void Write(byte data, byte Rs)
        {
            PulseEnable(Convert.ToByte((data & 0xf0) | (Rs << Hd44780.Rs)));
            PulseEnable(Convert.ToByte((data & 0x0f) << 4 | (Rs << Hd44780.Rs)));
        }


        /**
        * Create falling edge of "enable" pin to write data/inctruction to Display
        */
        private void PulseEnable(byte data)
        {
            _i2CPortExpander.Write(new[] { Convert.ToByte(data | (1 << En) | (_backLight << Bl)) }); // Enable bit HIGH
            _i2CPortExpander.Write(new[] { Convert.ToByte(data | (_backLight << Bl)) }); // Enable bit LOW
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
