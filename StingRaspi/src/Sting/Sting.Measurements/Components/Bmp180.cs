using System.Threading.Tasks;
using Sting.Measurements.Contracts;
using Sting.Measurements.External_Libraries;


namespace Sting.Measurements.Components
{
    class Bmp180 : IGpioComponent, ISensor
    {
        private BMP180Sensor _bmp;
        private readonly Resolution _resolution;

        /// <summary>
        /// Represents a BMP180 Sensor.
        /// </summary>
        /// <param name="res">Determines the used resolution of the sensor.
        /// The higher the resolution the more reliable the measurement results.
        /// As default UltraHighResolution is used.</param>
        public Bmp180(Resolution res = Resolution.UltrHighResolution)
        {
            _resolution = res;
        }

        /// <inheritdoc />
        public async Task<bool> InitComponentAsync(int pin = 0)
        {
            if(!State())
                _bmp = new BMP180Sensor(_resolution);

            await _bmp.InitializeAsync();
            return State();
        }

        /// <inheritdoc />
        public bool State()
        {
            return _bmp != null;
        }

        /// <inheritdoc />
        public void ClosePin()
        {
            _bmp.Dispose();
            _bmp = null;
        }

        /// <summary>
        /// Takes a measurement of temperature and air pressure. Adds the current timestamp.
        /// </summary>
        /// <returns>Returns TelemetryData if measurement is valid otherwise returns null.</returns>
        public async Task<TelemetryData> TakeMeasurementAsync()
        {
            if (!State()) return null;
            var data = await _bmp.ReadAsync();
            return new TelemetryData();         // TODO: return BMP180Data Type
        }
    }
}
