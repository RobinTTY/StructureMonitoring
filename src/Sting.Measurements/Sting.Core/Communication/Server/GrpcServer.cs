using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Sting.Core.Contracts;

namespace Sting.Core.Communication.Server
{
    public class GrpcServer : IService
    {
        public State State { get; set; }
        private readonly IHost _server;

        public GrpcServer()
        {
            _server = Host.CreateDefaultBuilder().ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            }).Build();
        }

        public void Start() => _server.StartAsync();

        public void Stop() => _server.StopAsync();
    }
}
