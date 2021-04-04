using Microsoft.Extensions.Logging;
using Netension.Authorization.OAuth.Clients;
using Netension.Authorization.OAuth.Options;
using Netension.Authorization.OAuth.Storages;
using Netension.Authorization.OAuth.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OAuth.Authenticators
{
    public class ClientCredentialsAuthenticator : IAuthenticator
    {
        private readonly ClientCredentialsOptions _options;
        private readonly IOAuthClient _client;
        private readonly ITokenStorage _storage;
        private readonly ILogger<ClientCredentialsAuthenticator> _logger;

        public ClientCredentialsAuthenticator(ClientCredentialsOptions options, IOAuthClient client, ITokenStorage storage, ILogger<ClientCredentialsAuthenticator> logger)
        {
            _options = options;
            _client = client;
            _storage = storage;
            _logger = logger;
        }

        public async Task<string> AuthenticateAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Authenticate {client} client", _options.ClientId);

            var token = await _storage.GetAccessTokenAsync(cancellationToken);
            if (token is null)
            {
                var tokens = await _client.CallTokenEndpointAsync(_options.TokenEndpoint, new ClientCredentialsRequest(_options.ClientId, _options.ClientSecret, string.Join(' ', _options.Scopes)), cancellationToken);
                await _storage.StoreAccessTokenAsync(tokens.AccessToken, tokens.ExpiresIn, cancellationToken);
                token = tokens.AccessToken;
            }

            return token;
        }
    }
}
