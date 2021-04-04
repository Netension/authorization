using Microsoft.Extensions.Logging;
using Netension.Authorization.OAuth.Binders;
using Netension.Authorization.OAuth.ValueObjects;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OAuth.Clients
{
    public static class OAuthDefaults
    {
        public static string DiscoveryPath { get; } = "/.well-known/openid-configuration";
    }

    public class OAuthClient : IOAuthClient
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenRequestBinder _tokenRequestBinder;
        private readonly ILogger<OAuthClient> _logger;

        public OAuthClient(HttpClient httpClient, ITokenRequestBinder tokenRequestBinder, ILogger<OAuthClient> logger)
        {
            _httpClient = httpClient;
            _tokenRequestBinder = tokenRequestBinder;
            _logger = logger;
        }

        public async Task<TokenResponse> AuthorizeAsync(Uri tokenEndpoint, ClientCredentialsRequest request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Authorize with {flow} flow", "Client Credentials");

            var response = await _httpClient.SendAsync(_tokenRequestBinder.Bind(tokenEndpoint, request), cancellationToken);

            return await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: cancellationToken);
        }
    }
}
