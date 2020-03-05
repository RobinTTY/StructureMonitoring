using System.Device.Gpio;
using System.Threading.Tasks;
using Sting.Devices.BaseClasses;
using Sting.Devices.Configurations;
using Sting.Models.Configuration;

namespace Sting.Devices.Actuators
{
    // TODO: implement IDisposable to reset pin?
    public class LedController : DeviceBase
    {
        public LedState State => CheckState();

        private readonly GpioController _gpioController;
        private int _pinNumber;

        public LedController() => _gpioController = new GpioController();

        public void TurnOn() => _gpioController.Write(_pinNumber, PinValue.High);

        public void TurnOff() => _gpioController.Write(_pinNumber, PinValue.Low);

        public async Task BlinkAsync(int duration)
        {
            TurnOn();
            await Task.Delay(duration);
            TurnOff();
        }

        public override bool Configure(IDeviceConfiguration deviceConfiguration)
        {
            if (deviceConfiguration.GetType() != typeof(LedConfiguration))
                return false;

            var config = (LedConfiguration)deviceConfiguration;
            _pinNumber = config.PinNumber;
            _gpioController.OpenPin(_pinNumber, PinMode.Output);
            _gpioController.Write(_pinNumber, PinValue.Low);

            return true;
        }

        private LedState CheckState()
        {
            var state = _gpioController.Read(_pinNumber);
            return state == PinValue.High ? LedState.On : LedState.Off;
        }
    }

    public enum LedState
    {
        On,
        Off
    }
}
