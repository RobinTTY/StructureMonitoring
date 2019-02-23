using System;
using System.Diagnostics;
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
    enum Resolution
    {
        UltraLowPower = 0,
        Standard = 1,
        HighResolution = 2,
        UltrHighResolution = 3
    }

    class Bmp180
    {
        private I2cDevice sensor;
        private const byte BMP180_ADDR = 0x77;
        private int oss;                                // Oversampling setting
        private int pressDelay;
        private bool calibrated;

        private Calibration _calibration;
        private const byte AddrCalibrationAc1 = 0xAA;
        private const byte AddrCalibrationAc2 = 0xAC;
        private const byte AddrCalibrationAc3 = 0xAE;
        private const byte AddrCalibrationAc4 = 0xB0;
        private const byte AddrCalibrationAc5 = 0xB2;
        private const byte AddrCalibrationAc6 = 0xB4;
        private const byte AddrCalibrationB1 = 0xB6;
        private const byte AddrCalibrationB2 = 0xB8;
        private const byte AddrCalibrationMb = 0xBA;
        private const byte AddrCalibrationMc = 0xBC;
        private const byte AddrCalibrationMd = 0xBE;

        /// <summary>
        /// Constructor
        /// </summary>
        public Bmp180(Resolution resolution)
        {
            oss = (short)resolution;
            switch (resolution)
            {
                case Resolution.UltraLowPower:
                    pressDelay = 5;
                    break;
                case Resolution.Standard:
                    pressDelay = 8;
                    break;
                case Resolution.HighResolution:
                    pressDelay = 14;
                    break;
                case Resolution.UltrHighResolution:
                    pressDelay = 26;
                    break;
            }
        }

        /// <summary>
        /// Initialize Bmp180 sensor
        /// </summary>
        public async Task InitializeAsync()
        {
            if (calibrated) return;
            var settings = new I2cConnectionSettings(BMP180_ADDR) { BusSpeed = I2cBusSpeed.StandardMode };
            var controller = await I2cController.GetDefaultAsync();

            sensor = controller.GetDevice(settings);
            ReadCalibrationData();
        }

        /// <summary>
        /// Read data from Bmp180 sensor
        /// </summary>
        /// <returns>BMP180Data</returns>
        public async Task<Bmp180Data> ReadAsync()
        {
            double temperature;
            double pressure;
            double altitude;
            double UT = await ReadUncompensatedTempDataAsync();
            double UP = await ReadUncompensatedPressDataAsync();

            CalculateTrueData(UT, UP, out temperature, out pressure);

            altitude = 44330 * (1 - Math.Pow(pressure / 101325, 0.1903));
            Debug.WriteLine( "Temp adjusted altitude:" + (Math.Pow(101325 / pressure, 0.190223) - 1) * (temperature + 273.15) / 0.0065);
            return new Bmp180Data(temperature, pressure, altitude);
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        public void Dispose()
        {
            sensor.Dispose();
        }

        /// <summary>
        /// Get Bmp180 Device
        /// </summary>
        /// <returns></returns>
        public I2cDevice GetDevice()
        {
            return sensor;
        }

        #region Private Methods

        /// <summary>
        /// Read calibration data from sensor
        /// </summary>
        private void ReadCalibrationData()
        {
            byte[] readBuf = new byte[2];

            sensor.WriteRead(new[] { AddrCalibrationAc1 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Ac1 = BitConverter.ToInt16(readBuf, 0);

            sensor.WriteRead(new[] { AddrCalibrationAc2 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Ac2 = BitConverter.ToInt16(readBuf, 0);

            sensor.WriteRead(new[] { AddrCalibrationAc3 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Ac3 = BitConverter.ToInt16(readBuf, 0);

            sensor.WriteRead(new[] { AddrCalibrationAc4 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Ac4 = BitConverter.ToUInt16(readBuf, 0);

            sensor.WriteRead(new[] { AddrCalibrationAc5 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Ac5 = BitConverter.ToUInt16(readBuf, 0);

            sensor.WriteRead(new[] { AddrCalibrationAc6 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Ac6 = BitConverter.ToUInt16(readBuf, 0);

            sensor.WriteRead(new[] { AddrCalibrationB1 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.B1 = BitConverter.ToInt16(readBuf, 0);

            sensor.WriteRead(new[] { AddrCalibrationB2 }, readBuf);
            Array.Reverse(readBuf);
            _calibration.B2 = BitConverter.ToInt16(readBuf, 0);

            sensor.WriteRead(new[] { AddrCalibrationMb }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Mb = BitConverter.ToInt16(readBuf, 0);

            sensor.WriteRead(new[] { AddrCalibrationMc }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Mc = BitConverter.ToInt16(readBuf, 0);

            sensor.WriteRead(new[] { AddrCalibrationMd }, readBuf);
            Array.Reverse(readBuf);
            _calibration.Md = BitConverter.ToInt16(readBuf, 0);
            calibrated = true;
        }

        /// <summary>
        /// Get uncompensated temperature
        /// </summary>
        /// <returns>Uncompensated temperature</returns>
        private async Task<double> ReadUncompensatedTempDataAsync()
        {
            var readBuf = new byte[2];

            sensor.Write(new byte[] { 0xF4, 0x2E });
            await Task.Delay(5);
            sensor.WriteRead(new byte[] { 0xF6 }, readBuf);

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
            var op = (byte)(0x34 + (oss << 6));

            sensor.Write(new byte[] { 0xF4, op });
            await Task.Delay(pressDelay);
            sensor.WriteRead(new byte[] { 0xF6 }, readBuf);

            var up = (readBuf[0] * Math.Pow(2, 16) + readBuf[1] * Math.Pow(2, 8) + readBuf[2]) / Math.Pow(2, (8 - oss));
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
            var b3 = ((_calibration.Ac1 * 4 + x3) * Math.Pow(2, oss) + 2) / 4;
            x1 = _calibration.Ac3 * b6 / Math.Pow(2, 13);
            x2 = (_calibration.B1 * (b6 * b6 / Math.Pow(2, 12))) / Math.Pow(2, 16);
            x3 = ((x1 + x2) + 2) / 4;
            var b4 = _calibration.Ac4 * (ulong)(x3 + 32768) / Math.Pow(2, 15);
            var b7 = ((ulong)up - b3) * (50000 / Math.Pow(2, oss));

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

        #endregion
    }
}