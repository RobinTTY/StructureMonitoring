using System;
using Sting.Core.Contracts;

namespace Sting.Core.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message) => Console.Write(message);
    }
}
