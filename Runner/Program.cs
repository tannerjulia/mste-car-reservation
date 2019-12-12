using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using AutoReservation.Service.Grpc;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Hosting;

namespace Runner
{
    class Program
    {
       // private static readonly IHost _host;

        static void Main(string[] args)
        {
            IHost _host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseUrls("https://localhost:50001")
                        .UseStartup<Startup>();
                })
                .Build();

            _host.Start();
        }
    }
}
