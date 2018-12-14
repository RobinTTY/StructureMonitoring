using System.Diagnostics;
using System.Drawing;
using System.IO;
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

        /// <inheritdoc />
        public Task<bool> InitComponentAsync(int pin = 0)
        {
            if(!State())
                _lcd = new Display(DeviceI2CAddress, I2CControllerName, Rs, Rw, En, D4, D5, D6, D7, Bl);
            try
            {
                _lcd.Init();
                SetCursorPosition(1);
            }
            catch (FileNotFoundException e)
            {
                _lcd.Dispose();
                _lcd = null;
                Debug.WriteLine("Lcd couldn't be initialized, check connections");
            }
            return Task.FromResult(State());
        }

        /// <inheritdoc />
        public bool State()
        {
            return _lcd != null;
        }

        /// <inheritdoc />
        public void ClosePin()
        {
            if (!State()) return;
            _lcd.ClrScr();
            _lcd.Dispose();
            _lcd = null;
        }

        /// <summary>
        /// Toggles the backlight of the lcd.
        /// </summary>
        public void ToggleBacklight()
        {
            if (!State()) return;
            if (_lcd.BacklightStatus())
            {
                _lcd.TurnOffBacklight();
                return;
            }
            _lcd.TurnOnBacklight();
        }

        /// <summary>
        /// Sets the position of the writing cursor on the lcd display.
        /// The cursor position isn't changed if an invalid parameter is passed.
        /// </summary>
        /// <param name="line">The line number. 1 for the first line, 2 for the second line.</param>
        /// <param name="column">The column number. Between 1 and 16. Default is 1.</param>
        public void SetCursorPosition(int line, int column = 1)
        {
            if (!State()) return;
            line--; column--;
            if (line < 0 || column < 0 || line > 1 || column > 15) return;
           _lcd.GoToXy(column, line);
        }

        /// <summary>
        /// Writes a string to the lcd at the current cursor position.
        /// </summary>
        /// <param name="msg">String that is to be written to the lcd.</param>
        public void Write(string msg)
        {
            if (!State()) return;
            _lcd.Prints(msg);
        }

        /// <summary>
        /// Clears the lcd of all currently displayed characters.
        /// </summary>
        /// <param name="resetCursor">If set to false doesn't reset the cursor position to the first line, first character.</param>
        public void ClearScreen(bool resetCursor = true)
        {
            if (!State()) return;
            _lcd.ClrScr();
            if(resetCursor) SetCursorPosition(1);
        }
    }
}
