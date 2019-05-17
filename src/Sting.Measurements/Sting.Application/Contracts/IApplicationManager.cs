namespace Sting.Application.Contracts
{
    public interface IApplicationManager
    {
        /// <summary>
        /// Starts the application.
        /// </summary>
        void StartApplication();

        /// <summary>
        /// Stops the application.
        /// </summary>
        void StopApplication();
    }
}
