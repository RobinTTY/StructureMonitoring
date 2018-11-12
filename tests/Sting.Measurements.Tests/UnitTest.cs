using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sting.Measurements.Components;

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
                {Altitude = 200, Humidity = 53, Temperature = 23, Pressure = 1700, Timestamp = DateTime.MaxValue};
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
    public class DhtTest
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
}
