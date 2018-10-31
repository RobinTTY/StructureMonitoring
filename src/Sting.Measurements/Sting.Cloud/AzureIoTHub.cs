using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using DotNetty.Transport.Channels;
using Microsoft.Azure.Devices.Client;

namespace Sting.Cloud
{
    public class AzureIotHub
    {
        private readonly string _connectionString;

        /// <summary>
        /// If given string is a file path of an existing file, the contents of the file will be used as connection string.
        /// If given string is not the file path to an existing file, the string will be set as connection string itself.
        /// </summary>
        /// <param name="connectionString">File path or connection string.</param>
        public AzureIotHub(string connectionString = "C:\\Data\\Users\\DefaultAccount\\AppData\\Local\\Packages\\Sting.Measurements-uwp_gk6cf97c3a7py\\LocalState\\DeviceConnectionString.txt")
        {
            _connectionString = System.IO.File.Exists(connectionString) ? System.IO.File.ReadAllText(connectionString) : connectionString;
        }

        /// <summary>
        /// Sends a given string to the IoT Hub which the connection string points to.
        /// </summary>
        /// <param name="msg">A String of variable length.</param>
        public async Task SendDeviceToCloudMessage(string msg)
        {
            var deviceClient = DeviceClient.CreateFromConnectionString(_connectionString, TransportType.Mqtt);
            var message = new Message(Encoding.ASCII.GetBytes(msg));
            
            await deviceClient.SendEventAsync(message);
            deviceClient.Dispose();
        }
    }
}
