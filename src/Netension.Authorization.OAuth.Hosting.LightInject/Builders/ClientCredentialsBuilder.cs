using LightInject;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Netension.Authorization.OAuth.Binders;
using Netension.Authorization.OAuth.Storages;

namespace Netension.Authorization.OAuth.Hosting.LightInject.Builders
{
    public class ClientCredentialsBuilder
    {
        private readonly IHostBuilder _hostBuilder;
        private readonly string _scheme;

        public ClientCredentialsBuilder(IHostBuilder hostBuilder, string scheme)
        {
            _hostBuilder = hostBuilder;
            _scheme = scheme;
        }

        public void SendCredentialsInHeader()
        {
            _hostBuilder.ConfigureContainer<IServiceContainer>((context, container) => container.RegisterTransient<ITokenRequestBinder, HeaderTokenRequestBinder>(_scheme));
        }

        public void SendCredentialsInBody()
        {
            _hostBuilder.ConfigureContainer<IServiceContainer>((context, container) => container.RegisterTransient<ITokenRequestBinder, BodyTokenRequestBinder>(_scheme));
        }

        public void UseDistributedTokenStorage()
        {
            _hostBuilder.ConfigureContainer<IServiceContainer>((context, container) => container.Register<ITokenStorage>(factory => new DistributedTokenStorage(_scheme, factory.GetInstance<IDistributedCache>(), factory.GetInstance<ILogger<DistributedTokenStorage>>()), _scheme, new PerContainerLifetime()));
        }

        public void UseMemoryTokenStorage()
        {
            _hostBuilder.ConfigureContainer<IServiceContainer>((context, container) => container.RegisterSingleton<ITokenStorage, MemoryTokenStorage>(_scheme));
        }
    }
}
