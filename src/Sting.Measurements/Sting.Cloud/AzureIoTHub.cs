using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;

namespace Sting.Cloud
{
    public class AzureIotHub
    {
        private const string ConnectionString = "{connection string}";

        /// <summary>
        /// Sends a given string to the IoT Hub which the connection string points to.
        /// </summary>
        /// <param name="msg">A String of variable length.</param>
        public static async Task SendDeviceToCloudMessage(string msg)
        {
            
            var deviceClient = DeviceClient.CreateFromConnectionString(ConnectionString, TransportType.Mqtt);
            var message = new Message(Encoding.ASCII.GetBytes(msg));

            await deviceClient.SendEventAsync(message);
        }
    }
}
