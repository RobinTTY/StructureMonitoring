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
        void PlayTone(double frequency, int duration);

        /// <summary>
        /// Set new or overwrite previously set frequency and start playing the sound.
        /// </summary>
        /// <param name="frequency">Tone frequency in Hertz.</param>
        void StartPlaying(double frequency);

        /// <summary>
        /// Stops playing the current tone.
        /// </summary>
        void StopPlaying();
    }
}
