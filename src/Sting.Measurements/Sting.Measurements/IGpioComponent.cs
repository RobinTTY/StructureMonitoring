namespace Sting.Measurements
{
    interface IGpioComponent
    {
        /// <summary>
        /// Initiates the sensor component, making it ready for use.
        /// </summary>
        /// <param name="pin">The data pin the sensor component uses.</param>
        /// <returns>Returns True if initiation was successful.</returns>
        bool InitSensor(int pin);

        /// <summary>
        /// Returns the state of the sensor component.
        /// </summary>
        /// <returns>Returns True if component is on.</returns>
        bool State();
    }
}
