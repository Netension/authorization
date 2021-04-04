using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OAuth.Storages
{
    public class MemoryTokenStorage : ITokenStorage
    {
        private readonly ILogger<MemoryTokenStorage> _logger;
        private string _accessToken;
        private DateTime? _expiredAt;

        public MemoryTokenStorage(ILogger<MemoryTokenStorage> logger)
        {
            _logger = logger;
        }

        public Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            if (DateTime.UtcNow > _expiredAt) return Task.FromResult<string>(null);
            return Task.FromResult(_accessToken);
        }

        public Task StoreAccessTokenAsync(string token, CancellationToken cancellationToken)
        {
            return StoreAccessTokenAsync(token, null, cancellationToken);
        }

        public Task StoreAccessTokenAsync(string token, TimeSpan? expiration, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Store access token");

            _accessToken = token;
            if (expiration.HasValue) _expiredAt = DateTime.UtcNow.Add(expiration.Value);

            return Task.CompletedTask;
        }
    }
}
