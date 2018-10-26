using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sting.Measurements
{
    interface ISensor
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
