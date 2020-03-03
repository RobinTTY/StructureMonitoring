using Iot.Device.Buzzer;
using Sting.Devices.Configurations;
using Sting.Devices.Contracts;
using Sting.Models.Configuration;

namespace Sting.Devices.Actuators
{
    public class BuzzerController : IDevice
    {
        // TODO: DeviceBase
        public string DeviceName { get; set; }
        private Buzzer _buzzer;

        public bool Configure(IDeviceConfiguration deviceConfiguration)
        {
            if (deviceConfiguration.GetType() != typeof(BuzzerConfiguration))
                return false;

            var config = (BuzzerConfiguration) deviceConfiguration;
            _buzzer = new Buzzer(config.PinNumber, config.PwmChannel);

            return true;
        }
    }
}
