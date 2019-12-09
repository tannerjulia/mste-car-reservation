using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Hosting;

namespace AutoReservation.Service.Grpc.Testing.Common
{
    public class ServiceTestFixture
        : IDisposable
    {
        private readonly IHost _host;
        public GrpcChannel Channel { get; }

        public ServiceTestFixture()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseUrls("https://localhost:50001")
                        .UseStartup<Startup>();
                })
                .Build();
            
            _host.Start();

           
            Channel = GrpcChannel.ForAddress(
                "https://localhost:50001", 
                new GrpcChannelOptions
                {
                    HttpClient = new HttpClient(
                        new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback =
                                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        }
                    )
                });
        }
        
        public void Dispose()
        {
            _host.Dispose();
        }
    }
}