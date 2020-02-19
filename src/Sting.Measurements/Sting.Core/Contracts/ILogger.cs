namespace Sting.Core.Contracts
{
    public interface ILogger
    {
        /// <summary>
        /// Logs a message to the designated output stream.
        /// </summary>
        /// <param name="message"></param>
        void Log(string message);
    }
}
