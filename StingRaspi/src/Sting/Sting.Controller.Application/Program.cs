namespace Sting.Application
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal static class Program
    {
        // TODO: change to Net Core equivalent
        // private readonly EasClientDeviceInformation _deviceInfo = new EasClientDeviceInformation();

        private static void Main(string[] args)
        {
            ApplicationManager.InitializeApplication();
            ApplicationManager.StartApplication();
        }
    }
}