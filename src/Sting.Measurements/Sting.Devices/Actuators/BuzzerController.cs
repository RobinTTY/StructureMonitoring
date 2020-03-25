using Iot.Device.Buzzer;
using Sting.Devices.BaseClasses;
using Sting.Devices.Configurations;

namespace Sting.Devices.Actuators
{
    public class BuzzerController : DeviceBase
    {
        // TODO: DeviceBase
        private Buzzer _buzzer;

        public override bool Configure(string jsonDeviceConfiguration)
        {
            var config = DeserializeDeviceConfig<BuzzerConfiguration>(jsonDeviceConfiguration);
            _buzzer = new Buzzer(config.PinNumber, config.PwmChannel);

            return true;
        }

        public void Beep() => _buzzer.PlayTone(440, 1000);
    }
}
