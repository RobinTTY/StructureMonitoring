using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Sting.Measurements.Tests
{
    [TestClass]
    public class TelemetryDataTest
    {
        [TestMethod]
        public void TelemetryObjectToJson()
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

        [TestMethod]
        public void TestMethodOn()
        {
            // turn Led on and wait for Hardware
            Led.InitSensor(5);
            Led.TurnOff();
            Led.TurnOn();
            Task.Delay(3 * 1000).Wait();
            var state = Led.State();
            Assert.IsTrue(state);
        }

        [TestMethod]
        public void TestMethodOff()
        {
            // turn Led off and wait for Hardware
            Led.TurnOn();
            Led.TurnOff();
            Task.Delay(3 * 1000).Wait();
            var state = Led.State();
            Assert.IsFalse(state);
        }
    }
}
