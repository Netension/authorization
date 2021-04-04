using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Netension.Authorization.OAuth.Hosting.LightInject;
using Serilog;
using System;

namespace Netension.Authorization.OAuth.NetCore.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
                .UseLightInject()
                .UseClientCredentialsAuthenticator("keycloak", (options, configuration) =>
                {
                    options.TokenEndpoint = new Uri(@"https://keycloak.mihben.net/auth/realms/Develop/protocol/openid-connect/token");
                    options.ClientId = "develop-client";
                    options.ClientSecret = "d495c459-682a-4c7f-88a2-5d82207d8184";
                }, builder =>
                {
                    builder.SendCredentialsInBody();
                    builder.UseMemoryTokenStorage();
                })

                .UseClientCredentialsAuthenticator("blizzard", (options, configuration) =>
                {
                    options.TokenEndpoint = new Uri(@"https://eu.battle.net/oauth/token");
                    options.ClientId = "e74e669060b7418aa8ca66ac7ba82395";
                    options.ClientSecret = "veq2ngqk3WhqFhq8iylHZm8FDFp0ZRh1";
                }, builder => 
                {
                    builder.SendCredentialsInHeader();
                    builder.UseMemoryTokenStorage();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
