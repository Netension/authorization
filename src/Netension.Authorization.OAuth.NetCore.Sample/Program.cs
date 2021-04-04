using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using Netension.Authorization.OAuth.Hosting.LightInject;
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
                .UseLightInject()
                .UseClientCredentialsAuthenticator("keycloak", (options, configuration) =>
                {
                    options.TokenEndpoint = new Uri(@"https://keycloak.mihben.net/auth/realms/Develop/protocol/openid-connect/token");
                    options.ClientId = "develop-client";
                    options.ClientSecret = "d495c459-682a-4c7f-88a2-5d82207d8184";
                }, builder => builder.SendCredentialsInBody())
                .UseClientCredentialsAuthenticator("blizzard", (options, configuration) =>
                {
                    options.TokenEndpoint = new Uri(@"https://eu.battle.net/oauth/token");
                    options.ClientId = "e74e669060b7418aa8ca66ac7ba82395";
                    options.ClientSecret = "veq2ngqk3WhqFhq8iylHZm8FDFp0ZRh1";
                }, builder => builder.SendCredentialsInHeader())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
