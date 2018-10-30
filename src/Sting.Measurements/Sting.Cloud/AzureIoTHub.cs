using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
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

            try
            {
                await deviceClient.SendEventAsync(message);
            }
            catch (ClosedChannelException e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine("Message: " + e.Message);
                Debug.WriteLine("Source: " + e.Source);
            }
            deviceClient.Dispose();
        }
    }
}
