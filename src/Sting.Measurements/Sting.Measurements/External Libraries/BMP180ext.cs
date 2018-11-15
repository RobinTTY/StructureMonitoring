﻿/*
 * MIT License
 * Copyright(c) 2018 - Zhang Yuexin

 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace Sting.Measurements.External_Libraries
{
    /// <summary>
    /// Calibration coefficients
    /// </summary>
    struct Calibration
    {
        public short AC1;
        public short AC2;
        public short AC3;
        public ushort AC4;
        public ushort AC5;
        public ushort AC6;
        public short B1;
        public short B2;
        public short MB;
        public short MC;
        public short MD;
    }

    /// <summary>
    /// BMP180Sensor sensor data
    /// </summary>
    struct BMP180Data
    {
        /// <summary>
        /// Temperature : ℃
        /// </summary>
        public double Temperature;
        /// <summary>
        /// Pressure : Pa
        /// </summary>
        public double Pressure;
        /// <summary>
        /// (Inaccurate) Altitude : m 
        /// </summary>
        public double Altitude;
    }

    /// <summary>
    /// BMP180Sensor sensor pressure sampling accuracy modes
    /// </summary>
    enum Resolution
    {
        UltraLowPower = 0,
        Standard = 1,
        HighResolution = 2,
        UltrHighResolution = 3
    }

    class BMP180Sensor
    {
        private I2cDevice sensor;
        private const byte BMP180_ADDR = 0x77;
        private int oss;                                // Oversampling setting
        private int pressDelay;
        private bool calibrated;

        #region Calibration MSB
        private Calibration cal;
        private const byte ADDR_CALIBRATION_AC1 = 0xAA;
        private const byte ADDR_CALIBRATION_AC2 = 0xAC;
        private const byte ADDR_CALIBRATION_AC3 = 0xAE;
        private const byte ADDR_CALIBRATION_AC4 = 0xB0;
        private const byte ADDR_CALIBRATION_AC5 = 0xB2;
        private const byte ADDR_CALIBRATION_AC6 = 0xB4;
        private const byte ADDR_CALIBRATION_B1 = 0xB6;
        private const byte ADDR_CALIBRATION_B2 = 0xB8;
        private const byte ADDR_CALIBRATION_MB = 0xBA;
        private const byte ADDR_CALIBRATION_MC = 0xBC;
        private const byte ADDR_CALIBRATION_MD = 0xBE;
        #endregion 

        /// <summary>
        /// Constructor
        /// </summary>
        public BMP180Sensor(Resolution resolution)
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
        /// Initialize BMP180Sensor sensor
        /// </summary>
        public async Task InitializeAsync()
        {
            if (calibrated) return;
            var settings = new I2cConnectionSettings(BMP180_ADDR) {BusSpeed = I2cBusSpeed.StandardMode};
            var controller = await I2cController.GetDefaultAsync();

            sensor = controller.GetDevice(settings);
            ReadCalibrationData();
        }

        /// <summary>
        /// Read data from BMP180Sensor sensor
        /// </summary>
        /// <returns>BMP180Data</returns>
        public async Task<BMP180Data> ReadAsync()
        {
            BMP180Data data;
            double UT = await ReadUncompensatedTempDataAsync();
            double UP = await ReadUncompensatedPressDataAsync();

            CalculateTrueData(UT, UP, out data.Temperature, out data.Pressure);

            data.Altitude = 44330 * (1 - Math.Pow(data.Pressure / 101325, 0.1903));
            //data.Altitude = ((Math.Pow((101325 / data.Pressure), 0.190223) - 1) * (data.Temperature + 273.15)) / 0.0065;
            return data;
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        public void Dispose()
        {
            sensor.Dispose();
        }

        /// <summary>
        /// Get BMP180Sensor Device
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

            sensor.WriteRead(new byte[] { ADDR_CALIBRATION_AC1 }, readBuf);
            Array.Reverse(readBuf);
            cal.AC1 = BitConverter.ToInt16(readBuf, 0);

            sensor.WriteRead(new byte[] { ADDR_CALIBRATION_AC2 }, readBuf);
            Array.Reverse(readBuf);
            cal.AC2 = BitConverter.ToInt16(readBuf, 0);

            sensor.WriteRead(new byte[] { ADDR_CALIBRATION_AC3 }, readBuf);
            Array.Reverse(readBuf);
            cal.AC3 = BitConverter.ToInt16(readBuf, 0);

            sensor.WriteRead(new byte[] { ADDR_CALIBRATION_AC4 }, readBuf);
            Array.Reverse(readBuf);
            cal.AC4 = BitConverter.ToUInt16(readBuf, 0);

            sensor.WriteRead(new byte[] { ADDR_CALIBRATION_AC5 }, readBuf);
            Array.Reverse(readBuf);
            cal.AC5 = BitConverter.ToUInt16(readBuf, 0);

            sensor.WriteRead(new byte[] { ADDR_CALIBRATION_AC6 }, readBuf);
            Array.Reverse(readBuf);
            cal.AC6 = BitConverter.ToUInt16(readBuf, 0);

            sensor.WriteRead(new byte[] { ADDR_CALIBRATION_B1 }, readBuf);
            Array.Reverse(readBuf);
            cal.B1 = BitConverter.ToInt16(readBuf, 0);

            sensor.WriteRead(new byte[] { ADDR_CALIBRATION_B2 }, readBuf);
            Array.Reverse(readBuf);
            cal.B2 = BitConverter.ToInt16(readBuf, 0);

            sensor.WriteRead(new byte[] { ADDR_CALIBRATION_MB }, readBuf);
            Array.Reverse(readBuf);
            cal.MB = BitConverter.ToInt16(readBuf, 0);

            sensor.WriteRead(new byte[] { ADDR_CALIBRATION_MC }, readBuf);
            Array.Reverse(readBuf);
            cal.MC = BitConverter.ToInt16(readBuf, 0);

            sensor.WriteRead(new byte[] { ADDR_CALIBRATION_MD }, readBuf);
            Array.Reverse(readBuf);
            cal.MD = BitConverter.ToInt16(readBuf, 0);
            calibrated = true;
        }

        /// <summary>
        /// Get uncompensated temperature
        /// </summary>
        /// <returns>Uncompensated temperature</returns>
        private async Task<double> ReadUncompensatedTempDataAsync()
        {
            double UT;
            byte[] readBuf = new byte[2];

            sensor.Write(new byte[] { 0xF4, 0x2E });
            await Task.Delay(5);
            sensor.WriteRead(new byte[] { 0xF6 }, readBuf);

            UT = readBuf[0] * Math.Pow(2, 8) + readBuf[1];

            return UT;
        }

        /// <summary>
        /// Get uncompensated pressure
        /// </summary>
        /// <returns>Uncompensated pressure</returns>
        private async Task<double> ReadUncompensatedPressDataAsync()
        {
            double UP;
            byte[] readBuf = new byte[3];
            byte op = (byte)(0x34 + (oss << 6));

            sensor.Write(new byte[] { 0xF4, op });
            await Task.Delay(pressDelay);
            sensor.WriteRead(new byte[] { 0xF6 }, readBuf);

            UP = (readBuf[0] * Math.Pow(2, 16) + readBuf[1] * Math.Pow(2, 8) + readBuf[2]) / Math.Pow(2, (8 - oss));

            return UP;
        }

        /// <summary>
        /// Get true data by calculating
        /// </summary>
        /// <param name="UT">Uncompensated temperature</param>
        /// <param name="UP">Uncompensated pressure</param>
        /// <param name="T">Out true temperature</param>
        /// <param name="P">Out true pressure</param>
        private void CalculateTrueData(double UT, double UP, out double T, out double P)
        {
            double X1, X2, B5;
            // Get true temperature
            X1 = (UT - cal.AC6) * cal.AC5 / Math.Pow(2, 15);
            X2 = cal.MC * Math.Pow(2, 11) / (X1 + cal.MD);
            B5 = X1 + X2;
            T = (B5 + 8) / Math.Pow(2, 4) / 10;

            double B3, B4, B6, B7, X3, p;
            // Get true pressure
            B6 = B5 - 4000;
            X1 = (cal.B2 * (B6 * B6 / Math.Pow(2, 12))) / Math.Pow(2, 11);
            X2 = cal.AC2 * B6 / Math.Pow(2, 11);
            X3 = X1 + X2;
            B3 = ((cal.AC1 * 4 + X3) * Math.Pow(2, oss) + 2) / 4;
            X1 = cal.AC3 * B6 / Math.Pow(2, 13);
            X2 = (cal.B1 * (B6 * B6 / Math.Pow(2, 12))) / Math.Pow(2, 16);
            X3 = ((X1 + X2) + 2) / 4;
            B4 = cal.AC4 * (ulong)(X3 + 32768) / Math.Pow(2, 15);
            B7 = ((ulong)UP - B3) * (50000 / Math.Pow(2, oss));
            if (B7 < 0x80000000)
            {
                p = (B7 * 2) / B4;
            }
            else
            {
                p = (B7 / B4) * 2;
            }
            X1 = (p / Math.Pow(2, 8)) * (p / Math.Pow(2, 8));
            X1 = (X1 * 3038) / Math.Pow(2, 16);
            X2 = (-7357 * p) / Math.Pow(2, 16);
            P = p + (X1 + X2 + 3791) / 16;
        }

        #endregion
    }
}
