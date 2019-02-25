using System;
using System.Threading.Tasks;
using Windows.Devices.I2c;
using Sting.Units;

namespace Sting.Devices
{
    /// <summary>
    /// Calibration coefficients
    /// </summary>
    class Calibration
    {
        public short Ac1;
        public short Ac2;
        public short Ac3;
        public ushort Ac4;
        public ushort Ac5;
        public ushort Ac6;
        public short B1;
        public short B2;
        public short Mb;
        public short Mc;
        public short Md;
    }

    /// <summary>
    /// Bmp180 sensor pressure sampling accuracy modes
    /// </summary>
    public enum Resolution
    {
        UltraLowPower = 0,
        Standard = 1,
        HighResolution = 2,
        UltraHighResolution = 3
    }

    public class Bmp180
    {
        private I2cDevice _sensor;
        private const byte Bmp180Address = 0x77;
        private readonly int _oss;                                // Oversampling setting
        private readonly int _pressDelay;
        private bool _calibrated;

        private readonly Calibration _calibration;
        private const byte CalibrationAc1 = 0xAA;
        private const byte CalibrationAc2 = 0xAC;
        private const byte CalibrationAc3 = 0xAE;
        private const byte CalibrationAc4 = 0xB0;
        private const byte CalibrationAc5 = 0xB2;
        private const byte CalibrationAc6 = 0xB4;
        private const byte CalibrationB1 = 0xB6;
        private const byte CalibrationB2 = 0xB8;
        private const byte CalibrationMb = 0xBA;
        private const byte CalibrationMc = 0xBC;
        private const byte CalibrationMd = 0xBE;

        /// <summary>
        /// Constructor
        /// </summary>
        public Bmp180(Resolution resolution)
        {
            _calibration = new Calibration();
            _oss = (short)resolution;

            switch (resolution)
            {
                case Resolution.UltraLowPower:
                    _pressDelay = 5;
                    break;
                case Resolution.Standard:
                    _pressDelay = 8;
                    break;
                case Resolution.HighResolution:
                    _pressDelay = 14;
                    break;
                case Resolution.UltraHighResolution:
                    _pressDelay = 26;
                    break;
            }
        }

        /// <summary>
        /// Initialize Bmp180 sensor
        /// </summary>
        public async Task InitializeAsync()
        {
            if (_calibrated) return;
            var settings = new I2cConnectionSettings(Bmp180Address) { BusSpeed = I2cBusSpeed.StandardMode };
            var controller = await I2cController.GetDefaultAsync();

            _sensor = controller.GetDevice(settings);
            ReadCalibrationData();
        }

        /// <summary>
        /// Read data from Bmp180 sensor
        /// </summary>
        /// <returns>BMP180Data</returns>
        public async Task<TelemetryData> ReadAsync()
        {
            var ut = await ReadUncompensatedTempDataAsync();
            var up = await ReadUncompensatedPressDataAsync();
            CalculateTrueData(ut, up, out var temperature, out var pressure);

            return new TelemetryData(temperature, pressure: pressure);
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        public void Dispose()
        {
            _sensor.Dispose();
        }

        /// <summary>
        /// Get Bmp180 Device
        /// </summary>
        /// <returns></returns>
        public I2cDevice GetDevice()
        {
            return _sensor;
        }

        /// <summary>
        /// Read calibration data from sensor
        /// </summary>
        private void ReadCalibrationData()
        {
            var readBuf = new byte[2];

            _sensor.WriteRead(new[] { CalibrationAc1 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Ac1 = BitConverter.ToInt16(readBuf, 0);

            _sensor.WriteRead(new[] { CalibrationAc2 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Ac2 = BitConverter.ToInt16(readBuf, 0);

            _sensor.WriteRead(new[] { CalibrationAc3 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Ac3 = BitConverter.ToInt16(readBuf, 0);

            _sensor.WriteRead(new[] { CalibrationAc4 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Ac4 = BitConverter.ToUInt16(readBuf, 0);

            _sensor.WriteRead(new[] { CalibrationAc5 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Ac5 = BitConverter.ToUInt16(readBuf, 0);

            _sensor.WriteRead(new[] { CalibrationAc6 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Ac6 = BitConverter.ToUInt16(readBuf, 0);

            _sensor.WriteRead(new[] { CalibrationB1 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.B1 = BitConverter.ToInt16(readBuf, 0);

            _sensor.WriteRead(new[] { CalibrationB2 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.B2 = BitConverter.ToInt16(readBuf, 0);

            _sensor.WriteRead(new[] { CalibrationMb }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Mb = BitConverter.ToInt16(readBuf, 0);

            _sensor.WriteRead(new[] { CalibrationMc }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Mc = BitConverter.ToInt16(readBuf, 0);

            _sensor.WriteRead(new[] { CalibrationMd }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Md = BitConverter.ToInt16(readBuf, 0);
            _calibrated = true;
        }

        /// <summary>
        /// Get uncompensated temperature
        /// </summary>
        /// <returns>Uncompensated temperature</returns>
        private async Task<double> ReadUncompensatedTempDataAsync()
        {
            var readBuf = new byte[2];

            _sensor.Write(new byte[] { 0xF4, 0x2E });
            await Task.Delay(5);
            _sensor.WriteRead(new byte[] { 0xF6 }, readBuf);

            var ut = readBuf[0] * Math.Pow(2, 8) + readBuf[1];
            return ut;
        }

        /// <summary>
        /// Get uncompensated pressure
        /// </summary>
        /// <returns>Uncompensated pressure</returns>
        private async Task<double> ReadUncompensatedPressDataAsync()
        {
            var readBuf = new byte[3];
            var op = (byte)(0x34 + (_oss << 6));

            _sensor.Write(new byte[] { 0xF4, op });
            await Task.Delay(_pressDelay);
            _sensor.WriteRead(new byte[] { 0xF6 }, readBuf);

            var up = (readBuf[0] * Math.Pow(2, 16) + readBuf[1] * Math.Pow(2, 8) + readBuf[2]) / Math.Pow(2, (8 - _oss));
            return up;
        }

        /// <summary>
        /// Get true data by calculating
        /// </summary>
        /// <param name="ut">Uncompensated temperature</param>
        /// <param name="up">Uncompensated pressure</param>
        /// <param name="T">Out true temperature</param>
        /// <param name="P">Out true pressure</param>
        private void CalculateTrueData(double ut, double up, out double T, out double P)
        {
            // Get true temperature
            var x1 = (ut - _calibration.Ac6) * _calibration.Ac5 / Math.Pow(2, 15);
            var x2 = _calibration.Mc * Math.Pow(2, 11) / (x1 + _calibration.Md);
            var b5 = x1 + x2;
            T = (b5 + 8) / Math.Pow(2, 4) / 10;

            double p;
            // Get true pressure
            var b6 = b5 - 4000;
            x1 = (_calibration.B2 * (b6 * b6 / Math.Pow(2, 12))) / Math.Pow(2, 11);
            x2 = _calibration.Ac2 * b6 / Math.Pow(2, 11);
            var x3 = x1 + x2;
            var b3 = ((_calibration.Ac1 * 4 + x3) * Math.Pow(2, _oss) + 2) / 4;
            x1 = _calibration.Ac3 * b6 / Math.Pow(2, 13);
            x2 = (_calibration.B1 * (b6 * b6 / Math.Pow(2, 12))) / Math.Pow(2, 16);
            x3 = ((x1 + x2) + 2) / 4;
            var b4 = _calibration.Ac4 * (ulong)(x3 + 32768) / Math.Pow(2, 15);
            var b7 = ((ulong)up - b3) * (50000 / Math.Pow(2, _oss));

            if (b7 < 0x80000000)
            {
                p = (b7 * 2) / b4;
            }
            else
            {
                p = (b7 / b4) * 2;
            }

            x1 = (p / Math.Pow(2, 8)) * (p / Math.Pow(2, 8));
            x1 = (x1 * 3038) / Math.Pow(2, 16);
            x2 = (-7357 * p) / Math.Pow(2, 16);
            P = p + (x1 + x2 + 3791) / 16;
        }
    }
}