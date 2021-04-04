using LightInject;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netension.Authorization.OAuth.Authenticators;
using Netension.Authorization.OAuth.Binders;
using Netension.Authorization.OAuth.Clients;
using Netension.Authorization.OAuth.Hosting.LightInject.Builders;
using Netension.Authorization.OAuth.Options;
using System;
using System.Net.Http;

namespace Netension.Authorization.OAuth.Hosting.LightInject
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseClientCredentialsAuthenticator(this IHostBuilder hostBuilder, string scheme, Action<ClientCredentialsOptions, IConfiguration> configure, Action<ClientCredentialsBuilder> build)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddOptions<ClientCredentialsOptions>(scheme)
                    .Configure(configure);

                services.AddHttpClient(scheme);
            });

            hostBuilder.ConfigureContainer<IServiceContainer>((context, container) =>
            {
                container.RegisterTransient<IOAuthClient>(factory => new OAuthClient(factory.GetInstance<IHttpClientFactory>().CreateClient(scheme), factory.GetInstance<ITokenRequestBinder>(scheme), factory.GetInstance<ILogger<OAuthClient>>()), scheme);
                container.RegisterSingleton<IAuthenticator>(factory => new ClientCredentialsAuthenticator(factory.GetInstance<IOptionsSnapshot<ClientCredentialsOptions>>().Get(scheme), factory.GetInstance<IOAuthClient>(scheme), factory.GetInstance<ILogger<ClientCredentialsAuthenticator>>()), scheme);
            });

            build(new ClientCredentialsBuilder(hostBuilder, scheme));

            return hostBuilder;
        }
    }
}
