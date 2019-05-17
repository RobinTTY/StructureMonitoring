using System;

namespace Sting.Devices.Contracts
{
    public interface IBuzzerController : IDisposable
    {
        /// <summary>
        /// Plays a tone for the given duration.
        /// </summary>
        /// <param name="frequency">The desired frequency in hz.</param>
        /// <param name="duration">The desired length of play.</param>
        void PlayToneAsync(double frequency, int duration);

        /// <summary>
        /// Activate the buzzer with the given frequency.
        /// </summary>
        /// <param name="frequency">The desired frequency in hz.</param>
        void SetFrequency(double frequency);

        /// <summary>
        /// Turns the buzzer off.
        /// </summary>
        void TurnOff();
    }
}
