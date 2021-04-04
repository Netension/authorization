using System;
using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OAuth.Storages
{
    public interface ITokenStorage
    {
        Task StoreAccessTokenAsync(string token, CancellationToken cancellationToken);
        Task StoreAccessTokenAsync(string token, TimeSpan? expiration, CancellationToken cancellationToken);
        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken);
    }
}
