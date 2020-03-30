namespace Sting.Core
{
    /// <summary>
    /// Describes the state of a Service.
    /// </summary>
    public enum State
    {
        /// <summary>
        /// The service is in an undefined state. This shouldn't happen.
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// The service is currently running.
        /// </summary>
        Running = 1,
        /// <summary>
        /// The service is currently stopped.
        /// </summary>
        Stopped = 2
    }
}