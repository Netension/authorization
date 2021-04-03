using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Netension.Authorization.OIDC.Authenticators;
using Netension.Authorization.OIDC.Clients;
using Netension.Authorization.OIDC.Options;
using System;

namespace Netension.Authorization.OIDC.NetCore
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseAuthenticator(this IHostBuilder hostBuilder, Action<ClientCredentialsOptions> configure)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddOptions<ClientCredentialsOptions>()
                    .Configure(configure);

                services.AddHttpClient<IOIDCClient, OIDCClient>((provider, client) =>
                {
                    var options = provider.GetRequiredService<IOptions<ClientCredentialsOptions>>();
                    client.BaseAddress = options.Value.Authority;
                });
                services.AddSingleton<IAuthenticator, ClientCredentialsAuthenticator>();
            });

            return hostBuilder;
        }
    }
}
