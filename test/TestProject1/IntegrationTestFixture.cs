using Microsoft.AspNetCore.Builder;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TestProject1.Tests
{
    public class IntegrationTestFixture: IAsyncLifetime
    {
        /// <summary>
        /// The default X.Server root URL, including port.
        /// </summary>
        public string DefaultServerRootUrl { get; set; }

        private WebApplication BlazorAppServer { get; set; }

        private IMessageSink MessageSink { get; }


        public IntegrationTestFixture(IMessageSink messageSink)
        {
            MessageSink = messageSink;

            const string AspNetEnvironment = "ASPNETCORE_ENVIRONMENT";
            if(Environment.GetEnvironmentVariable(AspNetEnvironment) == null)
            {
                Environment.SetEnvironmentVariable(AspNetEnvironment, "Development");
            }

            //The value of this URL:port is hardcoded here for demonstration purposes. Also "--urls" is not used.
            //DefaultServerRootUrl = $http://localhost:{GetFreePortFromEphemeralRange()};
            DefaultServerRootUrl = "https://localhost:7001";

            var builder = BlazorApp1.Server.Program.CreateWebHostBuilder(new[] { "--urls", DefaultServerRootUrl });
            var app = builder.Build();
            BlazorAppServer = BlazorApp1.Server.Program.InternalMain(app);
        }


        public bool IsTest()
        {
            return true;
        }


        public async Task InitializeAsync()
        {
            //await Task.CompletedTask;
            try
            {
                await BlazorAppServer.StartAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }            
        }


        public Task DisposeAsync()
        {
            _ = BlazorAppServer.DisposeAsync();
            return Task.CompletedTask;
        }


        private static int GetFreePortFromEphemeralRange()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();

            return port;
        }
    }
}
