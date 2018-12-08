using System.Threading.Tasks;
using Sting.Measurements.External_Libraries;

namespace Sting.Measurements.Components
{
    class Lcd : IGpioComponent
    {
        private Display _lcd;
        private const string I2C_CONTROLLER_NAME = "I2C1"; //use for RPI2
        private const byte DEVICE_I2C_ADDRESS = 0x27; // 7-bit I2C address of the port expander

        //Setup pins
        private const byte EN = 0x02;
        private const byte RW = 0x01;
        private const byte RS = 0x00;
        private const byte D4 = 0x04;
        private const byte D5 = 0x05;
        private const byte D6 = 0x06;
        private const byte D7 = 0x07;
        private const byte BL = 0x03;


        public Task<bool> InitComponentAsync(int pin = 0)
        {
            _lcd = new Display(DEVICE_I2C_ADDRESS, I2C_CONTROLLER_NAME, RS, RW, EN, D4, D5, D6, D7, BL);
            _lcd.init();
            return Task.FromResult(State());
        }

        public bool State()
        {
            return _lcd != null;
        }

        public void ClosePin()
        {
            _lcd.Dispose();
            _lcd = null;
        }

        public void doSomething()
        {
            _lcd.createSymbol(new byte[] { 0x00, 0x00, 0x0A, 0x00, 0x11, 0x0E, 0x00, 0x00 }, 0x00);
            _lcd.prints("Good morning!");
            _lcd.gotoxy(0, 1);
            _lcd.printSymbol(0x00);
        }
    }
}
