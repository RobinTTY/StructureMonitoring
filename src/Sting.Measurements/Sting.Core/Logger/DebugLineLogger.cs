using System.Diagnostics;
using Sting.Core.Contracts;

namespace Sting.Core.Logger
{
    public class DebugLineLogger : ILogger
    {
        public void Log(string message) => Debug.Write(message);
    }
}
