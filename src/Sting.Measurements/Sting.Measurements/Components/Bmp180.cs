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

        /// <inheritdoc />
        public async Task<bool> InitComponentAsync(int pin = 0)
        {
            await _bmp.InitializeAsync();
            return _bmp.GetDevice() != null;
        }

        /// <inheritdoc />
        public bool State()
        {
            return _bmp.GetDevice() != null;
        }

        /// <summary>
        /// Takes a measurement of temperature and air pressure. Adds the current timestamp.
        /// </summary>
        /// <returns>Returns TelemetryData if measurement is valid otherwise returns null.</returns>
        public async Task<TelemetryData> TakeMeasurement()
        {
            if (!State()) return null;
            var data = await _bmp.ReadAsync();
            return new TelemetryData(data);
        }
    }
}
