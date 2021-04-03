using Microsoft.Extensions.Logging;
using Netension.Authorization.OIDC.ValueObjects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OIDC.Clients
{
    public static class OIDCDefaults
    {
        public static string DiscoveryPath { get; } = "/.well-known/openid-configuration";
    }

    public class OIDCClient : IOIDCClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OIDCClient> _logger;

        public OIDCClient(HttpClient httpClient, ILogger<OIDCClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<TokenResponse> AuthorizeAsync(ClientCredentialsRequest request, Configuration configuration, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Authorize with {flow} flow", "Client Credentials");

            _httpClient.DefaultRequestHeaders.Authorization = new BasicAuthenticationOAuthHeaderValue(request.ClientId, request.ClientSecret);            

            var response = await _httpClient.PostAsync(new Uri("https://eu.battle.net/oauth/token"), new FormUrlEncodedContent(new Dictionary<string, string>() { { "grant_type", request.GrantType } }), cancellationToken);

            var stringResponse = await response.Content.ReadAsStringAsync();

            return await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: cancellationToken);
        }

        public async Task<Configuration> DiscoverAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Discover {authority} endpoints", _httpClient.BaseAddress);
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}{OIDCDefaults.DiscoveryPath}", cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Configuration>(cancellationToken: cancellationToken);
        }

        public async Task<TokenResponse> RefreshAsync(RefreshTokenRequest request, Configuration configuration, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Refresh token");
            var response = await _httpClient.PostAsync(configuration.TokenEndpoint, JsonContent.Create(request), cancellationToken);

            return await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: cancellationToken);
        }
    }
}
