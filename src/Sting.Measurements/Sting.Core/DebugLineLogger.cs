using System.Diagnostics;
using Sting.Core.Contracts;

namespace Sting.Core
{
    public class DebugLineLogger : ILogger
    {
        public void Log(string message) => Debug.WriteLine(message);
    }
}
