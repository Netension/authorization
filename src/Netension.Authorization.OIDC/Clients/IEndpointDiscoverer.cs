using Netension.Authorization.OIDC.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OIDC.Clients
{
    public interface IEndpointDiscoverer
    {
        Task<Configuration> DiscoverAsync(CancellationToken cancellationToken);
    }
}
