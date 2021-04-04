using Microsoft.Extensions.Hosting;

namespace Netension.Authorization.OAuth.NetCore.Registers
{
    public class OAuthRegister
    {
        public IHostBuilder HostBuilder { get; }

        public OAuthRegister(IHostBuilder hostBuilder)
        {
            HostBuilder = hostBuilder;
        }
    }
}
