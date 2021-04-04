using Microsoft.Extensions.Logging;
using Netension.Authorization.OAuth.Clients;
using Netension.Authorization.OAuth.Options;
using Netension.Authorization.OAuth.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OAuth.Authenticators
{
    public class ClientCredentialsAuthenticator : IAuthenticator
    {
        private readonly ClientCredentialsOptions _options;
        private readonly IOAuthClient _client;
        private readonly ILogger<ClientCredentialsAuthenticator> _logger;

        private string _accessToken;

        public ClientCredentialsAuthenticator(ClientCredentialsOptions options, IOAuthClient client, ILogger<ClientCredentialsAuthenticator> logger)
        {
            _options = options;
            _client = client;
            _logger = logger;
        }

        public async Task<string> AuthenticateAsync(CancellationToken cancellationToken)
        {
            var tokens = await _client.AuthorizeAsync(_options.TokenEndpoint, new ClientCredentialsRequest(_options.ClientId, _options.ClientSecret, string.Join(' ', _options.Scopes)), cancellationToken);

            _accessToken = tokens.AccessToken;

            return _accessToken;
        }
    }
}
