using System.Drawing;
using System.Threading.Tasks;
using Sting.Measurements.External_Libraries;

namespace Sting.Measurements.Components
{
    class Lcd : IGpioComponent
    {
        private Display _lcd;
        private const string I2CControllerName = "I2C1";
        private const byte DeviceI2CAddress = 0x27;

        //Setup pins
        private const byte En = 0x02;
        private const byte Rw = 0x01;
        private const byte Rs = 0x00;
        private const byte D4 = 0x04;
        private const byte D5 = 0x05;
        private const byte D6 = 0x06;
        private const byte D7 = 0x07;
        private const byte Bl = 0x03;


        public Task<bool> InitComponentAsync(int pin = 0)
        {
            if(!State())
                _lcd = new Display(DeviceI2CAddress, I2CControllerName, Rs, Rw, En, D4, D5, D6, D7, Bl);
            _lcd.Init();
            SetCursorPosition(1);
            return Task.FromResult(State());
        }

        public bool State()
        {
            return _lcd != null;
        }

        public void ClosePin()
        {
            _lcd.ClrScr();
            _lcd.Dispose();
            _lcd = null;
        }

        public void ToggleBacklight()
        {
            if (_lcd.BacklightStatus())
            {
                _lcd.TurnOffBacklight();
                return;
            }
            _lcd.TurnOnBacklight();
        }

        public void SetCursorPosition(int line, int column = 1)
        {
            line--; column--;
            if (line < 0 || column < 0 || line > 1 || column > 1) return;
           _lcd.GoToXy(column, line);
        }

        public void Write(string msg)
        {
            _lcd.Prints(msg);
        }

        public void ClearScreen(bool resetCursor = true)
        {
            _lcd.ClrScr();
            if(resetCursor) SetCursorPosition(1);
        }
    }
}
