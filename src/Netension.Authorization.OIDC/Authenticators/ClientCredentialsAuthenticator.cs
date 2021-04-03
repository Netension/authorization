using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netension.Authorization.OIDC.Clients;
using Netension.Authorization.OIDC.Options;
using Netension.Authorization.OIDC.ValueObjects;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OIDC.Authenticators
{
    public class ClientCredentialsAuthenticator : IAuthenticator
    {
        private readonly IOptions<ClientCredentialsOptions> _options;
        private readonly IOIDCClient _client;
        private readonly ILogger<ClientCredentialsAuthenticator> _logger;

        private JwtSecurityToken _accessToken;
        private JwtSecurityToken _refreshToken;

        public ClientCredentialsAuthenticator(IOptions<ClientCredentialsOptions> options, IOIDCClient client, ILogger<ClientCredentialsAuthenticator> logger)
        {
            _options = options;
            _client = client;
            _logger = logger;
        }

        public async Task<string> AuthenticateAsync(CancellationToken cancellationToken)
        {
            //var configuration = await _client.DiscoverAsync(cancellationToken);
            var tokens = await _client.AuthorizeAsync(new ClientCredentialsRequest(_options.Value.ClientId, _options.Value.ClientSecret,  _options.Value.Scopes != null ? string.Join(' ', _options.Value.Scopes) : null), null, cancellationToken);

            _accessToken = tokens.AccessToken;
            _refreshToken = tokens.RefreshToken;

            return $"{_accessToken.EncodedHeader}.{_accessToken.EncodedPayload}.{_accessToken.RawSignature}";
        }
    }
}
