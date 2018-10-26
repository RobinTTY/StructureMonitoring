using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sting.Measurements
{
    interface ISensor
    {
        bool InitSensor(int pin);
    }
}
