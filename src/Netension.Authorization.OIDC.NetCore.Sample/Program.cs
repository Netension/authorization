using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace Netension.Authorization.OIDC.NetCore.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseAuthenticator(options =>
                {
                    options.Authority = new Uri(@"https://eu.battle.net");
                    options.ClientId = "e74e669060b7418aa8ca66ac7ba82395";
                    options.ClientSecret = "veq2ngqk3WhqFhq8iylHZm8FDFp0ZRh1";
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
