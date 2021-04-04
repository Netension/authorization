using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OAuth.NetCore.Sample.Services
{
    public class BlizzardClient : IBlizzardClient
    {
        private readonly HttpClient _httpClient;

        public BlizzardClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetActsAsync(CancellationToken cancellationToken)
        {
            return await (await _httpClient.GetAsync(new Uri("https://eu.api.blizzard.com/d3/data/act"), cancellationToken)).Content.ReadAsStringAsync(cancellationToken);
        }
    }
}
