using System.Threading.Tasks;

namespace Sting.Measurements.Contracts
{
    public interface IGpioComponent
    {
        /// <summary>
        /// Initiates the component, making it ready for use.
        /// </summary>
        /// <param name="pin">The data pin the component uses.
        /// If the component uses the I2C bus, the pin parameter is not required.</param>
        /// <returns>Returns True if initiation was successful.</returns>
        Task<bool> InitComponentAsync(int pin);

        /// <summary>
        /// Returns the state of the gpio component.
        /// </summary>
        /// <returns>Returns True if component is initiated.</returns>
        bool State();

        /// <summary>
        /// Closes the pin associated with the component, making it available for other devices.
        /// </summary>
        void ClosePin();
    }
}
