using System.Threading.Tasks;

namespace Sting.Devices
{
    public enum LedState
    {
        On,
        Off
    }

    public interface ILed
    {
        /// <summary>
        /// Gets the current state of the LED.
        /// </summary>
        LedState State { get; }

        /// <summary>
        /// Turns the LED on.
        /// </summary>
        /// <returns>Returns the state of the LED.</returns>
        void TurnOn();

        /// <summary>
        /// Turns the LED off.
        /// </summary>
        /// <returns>Returns the state of the LED.</returns>
       void TurnOff();

        /// <summary>
        /// Turns the LED on for a specified duration and then off again.
        /// </summary>
        /// <param name="duration">Duration in milliseconds.</param>
        Task BlinkAsync(int duration);
    }
}