namespace Sting.Core.Contracts
{
    public interface IService
    {
        /// <summary>
        /// Indicates whether the service is currently running.
        /// </summary>
        State State { get; set; }

        /// <summary>
        /// Starts the service.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the service.
        /// </summary>
        void Stop();
    }
}
