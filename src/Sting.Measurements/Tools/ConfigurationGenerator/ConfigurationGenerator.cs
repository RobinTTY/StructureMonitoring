using System.Collections.Generic;
using System.Text.Json;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;
using Sting.Core.Communication;
using Sting.Devices.Configurations;

namespace ConfigurationGenerator
{
    public static class ConfigurationGenerator
    {
        public static SystemConfiguration GenerateBasicSystemConfiguration()
        {
            return new SystemConfiguration
            {
                Metadata = new ConfigMetadata
                {
                    Version = "1.0.0"
                },
                ControllerConfig = new ControllerConfig
                {
                    Name = "RasPi_1",
                    ControllerType = ControllerType.RaspberryPi,
                    ControllerRole = ControllerRole.Coordinator
                },
                DatabaseConfig = new DatabaseConfig
                {
                    Type = "MongoDB",
                    Attributes = new DatabaseAttributesConfig
                    {
                        Name = "Sting",
                        ConnectionString = "test"
                    }
                }
            };
        }

        public static void AddBme680Config(this SystemConfiguration config)
        {
            var bme680Config = new Bme680Configuration
            {
                FilteringMode = Bme680FilteringMode.C3,
                GasConversionIsEnabled = true,
                HeaterIsEnabled = true,
                HeaterProfiles = new List<Bme680HeaterConfiguration>
                {
                    new Bme680HeaterConfiguration
                    {
                        HeaterProfile = Bme680HeaterProfile.Profile1,
                        Duration = 150,
                        TargetTemperature = 320
                    }
                },
                HumiditySampling = Sampling.HighResolution,
                PressureSampling = Sampling.HighResolution,
                TemperatureSampling = Sampling.HighResolution,
                ActiveProfile = Bme680HeaterProfile.Profile1,
                I2CAddress = Bme680.SecondaryI2cAddress
            };

            var bme680DeviceConfig = new DeviceConfig
            {
                Name = "Bme680",
                Configuration = JsonSerializer.Serialize(bme680Config)
            };

            config.DeviceConfig.Add(bme680DeviceConfig);
        }

        public static void AddBme280Config(this SystemConfiguration config)
        {
            var bme280Config = new Bme280Configuration
            {
                FilterMode = Bmx280FilteringMode.X2,
                HumiditySampling = Sampling.UltraHighResolution,
                PressureSampling = Sampling.UltraHighResolution,
                TemperatureSampling = Sampling.UltraHighResolution,
                I2CAddress = Bmx280Base.DefaultI2cAddress,
                StandbyTime = StandbyTime.Ms500
            };

            var bme280DeviceConfig = new DeviceConfig
            {
                Name = "Bme280",
                Configuration = JsonSerializer.Serialize(bme280Config)
            };

            config.DeviceConfig.Add(bme280DeviceConfig);
        }
    }
}
