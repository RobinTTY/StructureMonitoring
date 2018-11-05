using System;
using System.Threading.Tasks;
using BMP;

namespace Sting.Measurements.Components
{
    class Bmp180 : IGpioComponent
    {
        private BMP180Sensor _bmp;
        
        /// <summary>
        /// Represents a BMP180 Sensor.
        /// </summary>
        /// <param name="res">Determines the used resolution of the sensor.
        /// The higher the resolution the more reliable the measurement results.
        /// As default UltraHighResolution is used.</param>
        public Bmp180(Resolution res = Resolution.UltrHighResolution)
        {
            _bmp = new BMP180Sensor(res);
        }

        public Task<bool> InitComponentAsync(int pin)
        {
            throw new NotImplementedException();
        }

        public bool State()
        {
            throw new NotImplementedException();
        }
    }
}
