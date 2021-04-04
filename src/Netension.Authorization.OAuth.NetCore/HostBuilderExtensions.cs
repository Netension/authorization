using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Netension.Authorization.OAuth.Clients;
using Netension.Authorization.OAuth.NetCore.Registers;
using System;

namespace Netension.Authorization.OAuth.NetCore
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseOAuthClient(this IHostBuilder hostBuilder, Action<OAuthRegister> registrate)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddHttpClient<IOAuthClient, OAuthClient>();
            });

            registrate(new OAuthRegister(hostBuilder));

            return hostBuilder;
        }
    }
}
