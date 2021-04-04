using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Netension.Authorization.OAuth.Binders;
using Netension.Authorization.OAuth.Clients;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Netension.Authorization.Test.Clients
{
    public class OAuthClient_Test
    {
        private readonly ILogger<OAuthClient> _logger;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private Mock<ITokenRequestBinder> _tokenRequestBinderMock;
        private readonly Uri BASE_ADRESS = new Uri("http://test-authority");

        public OAuthClient_Test(ITestOutputHelper outputHelper)
        {
            _logger = new LoggerFactory()
                        .AddXUnit(outputHelper)
                        .CreateLogger<OAuthClient>();
        }

        private OAuthClient CreateSUT()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage());

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = BASE_ADRESS
            };

            _tokenRequestBinderMock = new Mock<ITokenRequestBinder>();

            return new OAuthClient(httpClient, _tokenRequestBinderMock.Object, _logger);
        }
    }

    public static class TestExtensions
    {

    }
}
