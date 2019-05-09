using System.Threading;
using System.Threading.Tasks;
using Iot.Device.Buzzer;

namespace Sting.Devices
{
    public class BuzzerController : IBuzzerController
    {
        private readonly Buzzer _buzzer;
        private CancellationTokenSource _source;

        public BuzzerController(int pinNumber, int pwmChannel = -1)
        {
            _buzzer = new Buzzer(pinNumber, pwmChannel);
        }

        public async void PlayToneAsync(double frequency, int duration)
        {
            _source = new CancellationTokenSource();
            SetFrequency(frequency);

            await Task.Delay(duration, _source.Token);
            _buzzer.StopPlaying();
        }

        public void SetFrequency(double frequency)
        {
            _buzzer.SetFrequency(frequency);
        }

        public void TurnOff()
        {
            _source?.Cancel();
            _buzzer.StopPlaying();
        }

        public void Dispose()
        {
            _buzzer?.Dispose();
        }
    }
}
