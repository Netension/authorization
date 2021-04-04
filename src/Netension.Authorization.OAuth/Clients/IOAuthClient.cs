using Netension.Authorization.OAuth.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OAuth.Clients
{
    public interface IOAuthClient
    {
        Task<TokenResponse> CallTokenEndpointAsync(Uri tokenEndpoint, ClientCredentialsRequest request, CancellationToken cancellationToken);
    }
}
