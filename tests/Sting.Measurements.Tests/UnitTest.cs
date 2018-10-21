using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sting.Measurements.Tests
{
    [TestClass]
    public class TelemetryDataTest
    {
        [TestMethod]
        public void TelemetryObjectToJson()
        {
            var telemetry = new TelemetryData {Temperature = 20.2, Humidity = 57, Timestamp = DateTime.Now};
            Assert.IsNotNull(telemetry.ToJson());
        }
    }
}
