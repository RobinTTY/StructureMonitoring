namespace ConfigurationGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationGenerator.GenerateBasicSystemConfiguration();
            config.AddBme280Config();
            config.AddBme680Config();
        }
    }
}
