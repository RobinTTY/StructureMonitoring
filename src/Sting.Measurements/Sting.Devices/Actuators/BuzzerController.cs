using Iot.Device.Buzzer;
using Sting.Devices.BaseClasses;
using Sting.Devices.Configurations;
using Sting.Models.Configuration;

namespace Sting.Devices.Actuators
{
    public class BuzzerController : DeviceBase
    {
        // TODO: DeviceBase
        private Buzzer _buzzer;

        public override bool Configure(IDeviceConfiguration deviceConfiguration)
        {
            if (deviceConfiguration.GetType() != typeof(BuzzerConfiguration))
                return false;

            var config = (BuzzerConfiguration) deviceConfiguration;
            _buzzer = new Buzzer(config.PinNumber, config.PwmChannel);

            return true;
        }
    }
}
