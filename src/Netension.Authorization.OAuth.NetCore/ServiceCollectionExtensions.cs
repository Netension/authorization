using Microsoft.Extensions.DependencyInjection;
using Netension.Authorization.OAuth.Authenticators;
using System;
using System.Net.Http.Headers;

namespace Netension.Authorization.OAuth.NetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddAuthenticatedHttpClient<TClient>(this IServiceCollection services, string scheme)
            where TClient : class
        {
            return services.AddHttpClient<TClient>((provider, client) =>
            {
                var authenticatorFactory = provider.GetRequiredService<Func<string, IAuthenticator>>();
                var authenticator = authenticatorFactory(scheme);
                var authenticateTask = authenticator.AuthenticateAsync(default);
                authenticateTask.Wait();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticateTask.Result);
            });
        }

        public static IHttpClientBuilder AddAuthenticatedHttpClient<TClient, TImplementation>(this IServiceCollection services, string scheme)
            where TClient : class
            where TImplementation : class, TClient
        {
            return services.AddHttpClient<TClient, TImplementation>((provider, client) =>
            {
                var authenticatorFactory = provider.GetRequiredService<Func<string, IAuthenticator>>();
                var authenticator = authenticatorFactory(scheme);
                var authenticateTask = authenticator.AuthenticateAsync(default);
                authenticateTask.Wait();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticateTask.Result);
            });
        }
    }
}
