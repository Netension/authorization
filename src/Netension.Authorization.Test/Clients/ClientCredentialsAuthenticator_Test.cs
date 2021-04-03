using Microsoft.Extensions.Logging;
using Netension.Authorization.OIDC.Authenticators;
using Xunit.Abstractions;

namespace Netension.Authorization.Test.Clients
{
    public class ClientCredentialsAuthenticator_TEst
    {
        private readonly ILogger<ClientCredentialsAuthenticator> _logger;

        public ClientCredentialsAuthenticator_TEst(ITestOutputHelper outputHelper)
        {
            _logger = new LoggerFactory()
                        .AddXUnit(outputHelper)
                        .CreateLogger<ClientCredentialsAuthenticator>();
        }

        //private ClientCredentialsAuthenticator CreateSUT()
        //{
        //    return new ClientCredentialsAuthenticator();
        //}
    }
}
