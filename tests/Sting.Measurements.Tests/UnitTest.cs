using System;
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
        public void ToJson()
        {
            var telemetry = new TelemetryData { Temperature = 20.2, Humidity = 57, Timestamp = DateTime.Now };
            var conversionResult = JsonConvert.SerializeObject(telemetry);
            Assert.AreEqual(telemetry.ToJson(), conversionResult);
        }

        [TestMethod]
        public void Complement()
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
    }

    [TestClass]
    public class LedTest
    {
        private static readonly Led Led = new Led();

        [TestMethod]
        public async void TestMethodOn()
        {
            // turn Led on and wait for Hardware
            await Led.InitComponentAsync(5);
            Led.TurnOff();
            Led.TurnOn();
            Task.Delay(3 * 1000).Wait();
            var state = Led.State();
            Assert.IsTrue(state);
        }

        [TestMethod]
        public async void TestMethodOff()
        {
            // turn Led off and wait for Hardware
            await Led.InitComponentAsync(5);
            Led.TurnOn();
            Led.TurnOff();
            Task.Delay(3 * 1000).Wait();
            var state = Led.State();
            Assert.IsFalse(state);
        }
    }
}
