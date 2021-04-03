using Netension.Authorization.OIDC.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OIDC.Clients
{
    public interface IOIDCClient
    {
        Task<Configuration> DiscoverAsync(CancellationToken cancellationToken);
        Task<TokenResponse> AuthorizeAsync(ClientCredentialsRequest request, Configuration configuration, CancellationToken cancellationToken);
        Task<TokenResponse> RefreshAsync(RefreshTokenRequest request, Configuration configuration, CancellationToken cancellationToken);
    }
}
