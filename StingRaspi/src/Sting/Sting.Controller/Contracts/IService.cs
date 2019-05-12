namespace Sting.Controller.Contracts
{
    public interface IService
    {
        /// <summary>
        /// Indicates whether the service is currently running.
        /// </summary>
        bool IsRunning { get; set; }

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
