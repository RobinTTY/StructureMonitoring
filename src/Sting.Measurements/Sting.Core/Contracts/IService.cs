namespace Sting.Core.Contracts
{
    public interface IService
    {
        /// <summary>
        /// Indicates the current <see cref="State"/> of the service.
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
