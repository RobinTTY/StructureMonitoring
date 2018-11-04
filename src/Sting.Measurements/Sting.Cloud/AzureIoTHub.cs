using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;

namespace Sting.Cloud
{
    public class AzureIotHub
    {
        private readonly string _connectionString;
        private readonly DeviceClient _deviceClient;

        /// <summary>
        /// Interacts with an Azure IoT Hub instance.
        /// If given string is a file path of an existing file, the contents of the file will be used as connection string.
        /// If given string is not the file path to an existing file, the string will be set as connection string itself.
        /// </summary>
        /// <param name="connectionString">File path or connection string.</param>
        public AzureIotHub(string connectionString = "C:\\Data\\Users\\DefaultAccount\\AppData\\Local\\Packages\\Sting.Measurements-uwp_gk6cf97c3a7py\\LocalState\\DeviceConnectionString.txt")
        {
            if (File.Exists(connectionString))
            {
                try {
                    _connectionString = File.ReadAllText(connectionString);
                }
                catch (UnauthorizedAccessException) { Debug.WriteLine("Insufficient read permissions."); }
                catch (Exception) { Debug.WriteLine("Error while reading connection string path.");}
            }
            else
                _connectionString = connectionString;

            _deviceClient = DeviceClient.CreateFromConnectionString(_connectionString, TransportType.Mqtt);
        }

        /// <summary>
        /// Sends a given string to the IoT Hub which the connection string points to.
        /// </summary>
        /// <param name="msg">A String of variable length.</param>
        /// <returns>Returns true if the message was successfully sent.</returns>
        public async Task<bool> SendDeviceToCloudMessageAsync(string msg)
        {
            var message = new Message(Encoding.ASCII.GetBytes(msg));

            try
            {
                await _deviceClient.SendEventAsync(message);
                return true;
            }
            catch (FormatException e)
            {
                Debug.WriteLine("Given connection string is not valid! Message was not sent.");
                Debug.WriteLine(e.StackTrace);
                return false;
            }
        }
    }
}
