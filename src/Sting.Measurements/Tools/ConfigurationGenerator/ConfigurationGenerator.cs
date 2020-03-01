using System.Collections.Generic;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;
using Sting.Devices.Configurations;
using Sting.Models.Configuration;

namespace ConfigurationGenerator
{
    public static class ConfigurationGenerator
    {
        public static SystemConfiguration GenerateBasicSystemConfiguration()
        {
            return new SystemConfiguration
            {
                Info = new ConfigInfo
                {
                    Version = "1.0.0"
                },
                Database = new ConfigDatabase()
                {
                    Type = "MongoDB",
                    Attributes = new ConfigDbAttributes
                    {
                        Name = "Sting",
                        ConnectionString = "test"
                    }
                },
                Devices = new List<ConfigDevice>()
            };
        }

        public static void AddBme680Config(this SystemConfiguration config)
        {
            config.Devices.Add(new ConfigDevice()
            {
                Name = "Bme680",
                Configuration = new Bme680Configuration()
                {
                    FilteringMode = Bme680FilteringMode.C3,
                    GasConversionIsEnabled = true,
                    HeaterIsEnabled = true,
                    HeaterProfiles = new List<Bme680HeaterConfiguration>
                    {
                        new Bme680HeaterConfiguration()
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
                }
            });
        }
    }
}
