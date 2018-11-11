using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
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
        private static readonly Led Led = new Led();

        [ClassInitialize]
        public static async Task ClassInit(TestContext context)
        {
            await Led.InitComponentAsync(5);
        }

        [TestMethod]
        public void TurnOn_CallOnLed_StateIsTrue()
        {
            // turn Led on and wait for Hardware
            Led.TurnOff();
            Task.Delay(1000).Wait();
            Led.TurnOn();
            Task.Delay(1000).Wait();
            var state = Led.State();
            Assert.IsTrue(state);
        }

        [TestMethod]
        public void TestMethodOff_CallOnLed_StateIsFalse()
        {
            // turn Led off and wait for Hardware
            Led.TurnOn();
            Task.Delay(1000).Wait();
            Led.TurnOff();
            Task.Delay(1000).Wait();
            var state = Led.State();
            Assert.IsFalse(state);
        }

        [TestMethod]
        [ExpectedException(typeof(FileLoadException))]
        public async Task InitComponentAsync_CalledSecondTimeOnSamePin_ThrowsException()
        {
            // component shouldn't be able to be initialized twice
            await Led.InitComponentAsync(5);
        }
    }

    [TestClass]
    public class DhtTest
    {
        [TestMethod]
        public async Task InitComponent_FirstCall_StateIsTrue()
        {
            var dht = new Dht11();
            Assert.IsFalse(dht.State());
            await dht.InitComponentAsync(4);
            Assert.IsTrue(dht.State());
        }
    }
}
