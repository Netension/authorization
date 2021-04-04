using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OAuth.Storages
{
    public class DistributedTokenStorage : ITokenStorage
    {
        private readonly string _key;
        private readonly IDistributedCache _cache;
        private readonly ILogger<DistributedTokenStorage> _logger;

        public DistributedTokenStorage(string key, IDistributedCache cache, ILogger<DistributedTokenStorage> logger)
        {
            _key = key;
            _cache = cache;
            _logger = logger;
        }

        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            var rawToken = await _cache.GetAsync($"{_key}-access_token", cancellationToken);
            if (rawToken is null || !rawToken.Any()) return null;

            return Encoding.UTF8.GetString(rawToken);
        }

        public Task StoreAccessTokenAsync(string token, CancellationToken cancellationToken)
        {
            return StoreAccessTokenAsync(token, null, cancellationToken);
        }

        public Task StoreAccessTokenAsync(string token, TimeSpan? expiration, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentNullException(nameof(token));

            return StoreAccessTokenInternalAsync(token, expiration, cancellationToken);
        }

        private async Task StoreAccessTokenInternalAsync(string token, TimeSpan? expiration, CancellationToken cancellationToken)
        {
            var cacheKey = $"{_key}-access_token";
            _logger.LogDebug("Store {key} acces token", cacheKey);

            await _cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(token), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration }, cancellationToken);
        }
    }
}
