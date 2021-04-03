using Microsoft.Extensions.Logging;
using Netension.Authorization.OIDC.ValueObjects;
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

    public class OIDCClient : IEndpointDiscoverer
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OIDCClient> _logger;

        public OIDCClient(HttpClient httpClient, ILogger<OIDCClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Configuration> DiscoverAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Discover {authority} endpoints", _httpClient.BaseAddress);
            var response = await _httpClient.GetAsync(OIDCDefaults.DiscoveryPath, cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Configuration>(cancellationToken: cancellationToken);
        }
    }
}
