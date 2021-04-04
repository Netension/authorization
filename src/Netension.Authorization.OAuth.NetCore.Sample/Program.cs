using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Netension.Authorization.OAuth.Hosting.LightInject;
using Serilog;

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
                .UseClientCredentialsAuthenticator("keycloak", "Keycloak", builder => builder.UseDistributedTokenStorage())
                .UseClientCredentialsAuthenticator("blizzard", "Blizzard", builder =>
                {
                    builder.SendCredentialsInHeader();
                    builder.UseDistributedTokenStorage();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
