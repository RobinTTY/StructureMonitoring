﻿using Iot.Device.DHTxx;
using Sting.Models;

namespace Sting.Devices
{
    public class DhtController : IDhtController
    {
        private readonly DhtSensor _dht;

        public DhtController(int pinNumber, DhtType type)
        {
            _dht = new DhtSensor(pinNumber, type);
        }

        public MeasurementContainer TakeMeasurement()
        {
            var temperature = _dht.Temperature.Celsius;
            var humidity = _dht.Humidity;

            return new MeasurementContainer(temperature, humidity);
        }

        public void Dispose()
        {
            _dht?.Dispose();
        }
    }
}