using LightInject;
using Microsoft.Extensions.Hosting;
using Netension.Authorization.OAuth.Binders;

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
    }
}
