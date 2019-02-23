﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sting.Measurements.Components;
using Sting.Cloud;

namespace Sting.Measurements.Tests
{
    [TestClass]
    public class TelemetryDataTest
    {
        [TestMethod]
        public void Complement_ValidParameter_FillsNaNValues()
        {
            var object1 = new TelemetryData {Temperature = 19.4, Altitude = 320.4};
            var object2 = new TelemetryData {Altitude = 100, Pressure = 5000};
            var objectResult = new TelemetryData {Temperature = 19.4, Altitude = 320.4, Pressure = 5000};

            object1.Complement(object2);
            Assert.AreEqual(object1.Temperature, objectResult.Temperature);
            Assert.AreEqual(object1.Humidity, objectResult.Humidity);
            Assert.AreEqual(object1.Pressure, objectResult.Pressure);
            Assert.AreEqual(object1.Altitude, objectResult.Altitude);
        }

        [TestMethod]
        public void Overwrite_ValidParameter_OverwritesMeasuredValues()
        {
            var object1 = new TelemetryData { Temperature = 19.4, Altitude = 320.4 };
            var object2 = new TelemetryData { Altitude = 100, Pressure = 5000 };
            var objectResult = new TelemetryData { Temperature = 19.4, Altitude = 100, Pressure = 5000 };

            object1.Overwrite(object2);
            Assert.AreEqual(object1.Temperature, objectResult.Temperature);
            Assert.AreEqual(object1.Humidity, objectResult.Humidity);
            Assert.AreEqual(object1.Pressure, objectResult.Pressure);
            Assert.AreEqual(object1.Altitude, objectResult.Altitude);
        }

        [TestMethod]
        public void ToString_CalledOnTelemetryData_CreatesValidString()
        {
            TelemetryData data = new TelemetryData()
                { Altitude = 200, Humidity = 53, Temperature = 23, Pressure = 1700, Timestamp = DateTime.MaxValue };
            var telemetryString = data.ToString();
            Assert.AreEqual(telemetryString, "Time: 12/31/9999 11:59:59 PM, Temperature: 23°C, Humidity: 53%, Pressure: 1700hPa, Altitude: 200m");
        }

            [TestMethod]
        public void ToJson_CalledOnTelemetryData_CreatesValidJson()
        {
            var telemetry = new TelemetryData { Temperature = 20.2, Humidity = 57, Timestamp = DateTime.Now };
            var conversionResult = JsonConvert.SerializeObject(telemetry);
            Assert.AreEqual(telemetry.ToJson(), conversionResult);
        }
    }

    [TestClass]
    public class LedTest
    {
        private readonly Led _led = new Led();

        [TestMethod]
        public async Task InitComponentAsync_CorrectCall_StateIsTrue()
        {
            await _led.InitComponentAsync(5);
            Assert.IsTrue(_led.State());
        }

        [TestMethod]
        public async Task TurnOn_CallOnLed_LedIsOn()
        {
            await _led.InitComponentAsync(5);
            _led.TurnOff();
            Task.Delay(1000).Wait();
            _led.TurnOn();
            Task.Delay(1000).Wait();
            Assert.IsTrue(_led.IsOn());
        }

        [TestMethod]
        public async Task TurnOff_CallOnLed_LedIsOff()
        {
            await _led.InitComponentAsync(5);
            _led.TurnOn();
            Task.Delay(1000).Wait();
            _led.TurnOff();
            Task.Delay(1000).Wait();
            Assert.IsFalse(_led.IsOn());
        }

        [TestMethod]
        [ExpectedException(typeof(FileLoadException))]
        public async Task InitComponentAsync_CalledSecondTimeOnSamePin_ThrowsException()
        {
            await _led.InitComponentAsync(5);
            await _led.InitComponentAsync(5);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if(_led.State())
                _led.ClosePin();
        }
    }

    [TestClass]
    public class Dht11Test
    {
        private readonly Dht11 _dht = new Dht11();

        [TestMethod]
        public async Task InitComponentAsync_CorrectCall_StateIsTrue()
        {
            Assert.IsFalse(_dht.State());
            await _dht.InitComponentAsync(4);
            Assert.IsTrue(_dht.State());
        }

        [TestMethod]
        public async Task TakeMeasurement_ComponentInitialized_MeasurementIsValid()
        {
            await _dht.InitComponentAsync(4);
            var measurement = await _dht.TakeMeasurementAsync();
            Assert.IsNotNull(measurement);
        }

        [TestMethod]
        public async Task TakeMeasurement_ComponentNotInitialized_MeasurementIsNull()
        {
            var measurement = await _dht.TakeMeasurementAsync();
            Assert.IsNull(measurement);
        }

        [TestMethod]
        [ExpectedException(typeof(FileLoadException))]
        public async Task InitComponent_InitTwice_ThrowsException()
        {
            await _dht.InitComponentAsync(4);
            await _dht.InitComponentAsync(4);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if(_dht.State())
                _dht.ClosePin();
        }
    }

    [TestClass]
    public class Bmp180Test
    {
        private readonly Bmp180 _bmp = new Bmp180();

        [TestMethod]
        public async Task InitComponentAsync_CorrectCall_StateIsTrue()
        {
            Assert.IsFalse(_bmp.State());
            await _bmp.InitComponentAsync();
            Assert.IsTrue(_bmp.State());
        }

        [TestMethod]
        public async Task InitComponentAsync_InitTwice_ComponentIsStillUsable()
        {
            var init1 = await _bmp.InitComponentAsync();
            var init2 = await _bmp.InitComponentAsync();
            var data = await _bmp.TakeMeasurementAsync();
            Assert.IsTrue(init1);
            Assert.IsTrue(init2);
            Assert.IsInstanceOfType(data, typeof(TelemetryData));
        }

        [TestMethod]
        public async Task TakeMeasurement_ComponentInitialized_MeasurementIsValid()
        {
            await _bmp.InitComponentAsync();
            var measurement = await _bmp.TakeMeasurementAsync();
            Assert.IsNotNull(measurement);
        }

        [TestMethod]
        public async Task TakeMeasurement_ComponentNotInitialized_MeasurementIsNull()
        {
            var measurement = await _bmp.TakeMeasurementAsync();
            Assert.IsNull(measurement);
        }

        [TestMethod]
        public async Task TakeMeasurement_InitializedAfterPinWasClosed_MeasurementIsValid()
        {
            await _bmp.InitComponentAsync();
            _bmp.ClosePin();
            await _bmp.InitComponentAsync();
            var measurement = await _bmp.TakeMeasurementAsync();
            Assert.IsInstanceOfType(measurement, typeof(TelemetryData));
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_bmp.State())
                _bmp.ClosePin();
        }
    }

    [TestClass]
    public class BuzzerTest
    {
        private readonly Buzzer _buzzer = new Buzzer();

        [TestMethod]
        public async Task InitComponentAsync_CorrectCall_StateIsTrue()
        {
            await _buzzer.InitComponentAsync(5);
            Assert.IsTrue(_buzzer.State());
        }

        [TestMethod]
        public async Task TurnOn_CallOnBuzzer_BuzzerIsOn()
        {
            await _buzzer.InitComponentAsync(18);
            _buzzer.TurnOff();
            Task.Delay(1000).Wait();
            _buzzer.TurnOn();
            Task.Delay(1000).Wait();
            Assert.IsTrue(_buzzer.IsOn());
        }

        [TestMethod]
        public async Task TurnOff_CallOnBuzzer_BuzzerIsOff()
        {
            await _buzzer.InitComponentAsync(18);
            _buzzer.TurnOn();
            Task.Delay(1000).Wait();
            _buzzer.TurnOff();
            Task.Delay(1000).Wait();
            Assert.IsFalse(_buzzer.IsOn());
        }

        [TestMethod]
        [ExpectedException(typeof(FileLoadException))]
        public async Task InitComponentAsync_CalledSecondTimeOnSamePin_ThrowsException()
        {
            await _buzzer.InitComponentAsync(18);
            await _buzzer.InitComponentAsync(18);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_buzzer.State())
                _buzzer.ClosePin();
        }
    }

    [TestClass]
    public class LcdTest
    {
        private readonly Lcd _lcd = new Lcd();

        [TestMethod]
        public async Task InitComponentAsync_CorrectCall_StateIsTrue()
        {
            Assert.IsFalse(_lcd.State());
            await _lcd.InitComponentAsync();
            Assert.IsTrue(_lcd.State());
        }

        [TestMethod]
        public async Task InitComponentAsync_CalledTwice_NoExceptionComponentUsable()
        {
            await _lcd.InitComponentAsync();
            var success = await _lcd.InitComponentAsync();
            Assert.IsTrue(success);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_lcd.State())
                _lcd.ClosePin();
        }
    }

    [TestClass]
    public class AzureIoTHubTest
    {
        private readonly AzureIotHub _myHub = new AzureIotHub("C:\\Data\\Users\\DefaultAccount\\AppData\\Local\\Packages\\Sting.Measurements.Tests-uwp_gk6cf97c3a7py\\LocalState\\DeviceConnectionString.txt");

        [TestMethod]
        public async Task SendDeviceToCloudMessageAsync_StringAsParameter_MessageIsSent()
        {
            var success = await _myHub.SendDeviceToCloudMessageAsync("This is a test message");
            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task SendDeviceToCloudMessageAsync_nullAsParameter_MessageIsNotSent()
        {
            var success = await _myHub.SendDeviceToCloudMessageAsync(null);
            Assert.IsFalse(success);
        }
    }
}