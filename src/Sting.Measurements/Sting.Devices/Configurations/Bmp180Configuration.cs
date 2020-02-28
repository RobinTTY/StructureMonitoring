﻿using Iot.Device.Bmp180;
using Sting.Models.Configuration;

namespace Sting.Devices.Configurations
{
    public class Bmp180Configuration : I2CDeviceConfiguration, IDeviceConfiguration
    {
        public Sampling Sampling { get; set; }
    }
}