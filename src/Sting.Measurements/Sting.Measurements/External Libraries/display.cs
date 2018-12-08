/**
 *  Character-LCD-over-I2C 
 *  ===================
 *  Connect HD44780 LCD character Display to Windows 10 IoT devices via I2C and PCF8574
 *
 *  Author: Jaroslav Zivny
 * Modified by: Robin Müller
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

        private const byte LCD_WRITE = 0x07;

        private byte _D4;
        private byte _D5;
        private byte _D6;
        private byte _D7;
        private byte _En;
        private byte _Rw;
        private byte _Rs;
        private byte _Bl;

        private readonly byte[] _lineAddress = { 0x00, 0x40 };

        private byte _backLight = 0x01;

        private I2cDevice _i2cPortExpander;


        public Display(byte deviceAddress, string controllerName, byte Rs, byte Rw, byte En, byte D4, byte D5, byte D6, byte D7, byte Bl, byte[] LineAddress) : this(deviceAddress, controllerName, Rs, Rw, En, D4, D5, D6, D7, Bl)
        {
            _lineAddress = LineAddress;
        }


        public Display(byte deviceAddress, string controllerName, byte Rs, byte Rw, byte En, byte D4, byte D5, byte D6, byte D7, byte Bl)
        {
            // Configure pins
            _Rs = Rs;
            _Rw = Rw;
            _En = En;
            _D4 = D4;
            _D5 = D5;
            _D6 = D6;
            _D7 = D7;
            _Bl = Bl;

            // It's async method, so we have to wait
            Task.Run(() => this.startI2C(deviceAddress, controllerName)).Wait();
        }



        /**
        * Start I2C Communication
        **/
        public async void startI2C(byte deviceAddress, string controllerName)
        {
            try
            {
                var i2cSettings = new I2cConnectionSettings(deviceAddress);
                i2cSettings.BusSpeed = I2cBusSpeed.FastMode;
                string deviceSelector = I2cDevice.GetDeviceSelector(controllerName);
                var i2cDeviceControllers = await DeviceInformation.FindAllAsync(deviceSelector);
                _i2cPortExpander = await I2cDevice.FromIdAsync(i2cDeviceControllers[0].Id, i2cSettings);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}", e.Message);
            }
        }


        /**
        * Initialization
        **/
        public void init(bool turnOnDisplay = true, bool turnOnCursor = false, bool blinkCursor = false, bool cursorDirection = true, bool textShift = false)
        {
            //Task.Delay(100).Wait();
            //pulseEnable(Convert.ToByte((turnOnBacklight << 3)));

            /* Init sequence */
            Task.Delay(100).Wait();
            pulseEnable(Convert.ToByte((1 << _D5) | (1 << _D4)));
            Task.Delay(5).Wait();
            pulseEnable(Convert.ToByte((1 << _D5) | (1 << _D4)));
            Task.Delay(5).Wait();
            pulseEnable(Convert.ToByte((1 << _D5) | (1 << _D4)));

            /*  Init 4-bit mode */
            pulseEnable(Convert.ToByte((1 << _D5)));

            /* Init 4-bit mode + 2 line */
            pulseEnable(Convert.ToByte((1 << _D5)));
            pulseEnable(Convert.ToByte((1 << _D7)));

            /* Turn on Display, cursor */
            pulseEnable(0);
            pulseEnable(Convert.ToByte((1 << _D7) | (Convert.ToByte(turnOnDisplay) << _D6) | (Convert.ToByte(turnOnCursor) << _D5) | (Convert.ToByte(blinkCursor) << this._D4)));

            clrscr();

            pulseEnable(0);
            pulseEnable(Convert.ToByte((1 << _D6) | (Convert.ToByte(cursorDirection) << _D5) | (Convert.ToByte(textShift) << _D4)));
        }


        /**
        * Turn the backlight ON.
        **/
        public void turnOnBacklight()
        {
            _backLight = 0x01;
            sendCommand(0x00);
        }


        /**
        * Turn the backlight OFF.
        **/
        public void turnOffBacklight()
        {
            _backLight = 0x00;
            sendCommand(0x00);
        }


        /**
        * Can print string onto Display
        **/
        public void prints(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                printc(text[i]);
            }
        }


        /**
        * Print single character onto Display
        **/
        public void printc(char letter)
        {
            try
            {
                write(Convert.ToByte(letter), 1);
            }
            catch (Exception e)
            {

            }
        }


        /**
        * skip to second line
        **/
        public void gotoSecondLine()
        {
            sendCommand(0xc0);
        }


        /**
        * goto X and Y 
        **/
        public void gotoxy(byte x, byte y)
        {
            sendCommand(Convert.ToByte(x | _lineAddress[y] | (1 << LCD_WRITE)));
        }


        /**
        * Send data to Display
        **/
        public void sendData(byte data)
        {
            write(data, 1);
        }


        /**
        * Send command to Display
        **/
        public void sendCommand(byte data)
        {
            write(data, 0);
        }


        /**
        * Clear Display and set cursor at start of the first line
        **/
        public void clrscr()
        {
            pulseEnable(0);
            pulseEnable(Convert.ToByte((1 << this._D4)));
            Task.Delay(5).Wait();
        }


        /**
        * Send pure data to Display
        **/
        public void write(byte data, byte Rs)
        {
            pulseEnable(Convert.ToByte((data & 0xf0) | (Rs << this._Rs)));
            pulseEnable(Convert.ToByte((data & 0x0f) << 4 | (Rs << this._Rs)));
            //Task.Delay(5).Wait(); //In case of problem with displaying wrong characters uncomment this part
        }


        /**
        * Create falling edge of "enable" pin to write data/inctruction to Display
        */
        private void pulseEnable(byte data)
        {
            _i2cPortExpander.Write(new[] { Convert.ToByte(data | (1 << _En) | (_backLight << _Bl)) }); // Enable bit HIGH
            _i2cPortExpander.Write(new[] { Convert.ToByte(data | (_backLight << _Bl)) }); // Enable bit LOW
            //Task.Delay(2).Wait(); //In case of problem with displaying wrong characters uncomment this part
        }


        /**
        * Save custom symbol to CGRAM
        **/
        public void createSymbol(byte[] data, byte address)
        {
            sendCommand(Convert.ToByte(0x40 | (address << 3)));
            for (var i = 0; i < data.Length; i++)
            {
                sendData(data[i]);
            }
            this.clrscr();
        }


        /**
        * Print custom symbol
        **/
        public void printSymbol(byte address)
        {
            sendData(address);
        }

        public void Dispose()
        {
            _i2cPortExpander.Dispose();
        }
    }
}