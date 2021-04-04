using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Netension.Authorization.OAuth.Binders;
using Netension.Authorization.OAuth.Clients;
using Netension.Authorization.OAuth.ValueObjects;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Netension.Authorization.Test.Clients
{
    public class OAuthClient_Test
    {
        private readonly ILogger<OAuthClient> _logger;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private Mock<ITokenRequestBinder> _tokenRequestBinderMock;

        public OAuthClient_Test(ITestOutputHelper outputHelper)
        {
            _logger = new LoggerFactory()
                        .AddXUnit(outputHelper)
                        .CreateLogger<OAuthClient>();
        }

        private OAuthClient CreateSUT()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _tokenRequestBinderMock = new Mock<ITokenRequestBinder>();

            return new OAuthClient(new HttpClient(_httpMessageHandlerMock.Object), _tokenRequestBinderMock.Object, _logger);
        }

        [Fact(DisplayName = "OAuthClient - AuthorizeAsync - Use request binder")]
        public async Task OAuthClient_AuthorizeAsync_UseRequestBinder()
        {
            // Arrange
            var sut = CreateSUT();
            var endpoint = new Fixture().Create<Uri>();
            var request = new Fixture().Create<ClientCredentialsRequest>();

            _tokenRequestBinderMock.Setup(trb => trb.Bind(It.IsAny<Uri>(), It.IsAny<ClientCredentialsRequest>()))
                .Returns(new HttpRequestMessage(HttpMethod.Get, endpoint));

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = JsonContent.Create(new Fixture().Create<TokenResponse>())
                });

            // Act
            await sut.AuthorizeAsync(endpoint, request, default);

            // Assert
            _tokenRequestBinderMock.Verify(trb => trb.Bind(It.Is<Uri>(u => u.Equals(endpoint)), It.Is<ClientCredentialsRequest>(ccr => ccr.Equals(request))), Times.Once);
        }

        [Fact(DisplayName = "OAuthClient - AuthorizeAsync - Parse response")]
        public async Task OAuthClient_AuthorizeAsync_ParseResponse()
        {
            // Arrange
            var sut = CreateSUT();
            var response = new Fixture().Create<TokenResponse>();

            _tokenRequestBinderMock.Setup(trb => trb.Bind(It.IsAny<Uri>(), It.IsAny<ClientCredentialsRequest>()))
                .Returns(new HttpRequestMessage(HttpMethod.Get, new Fixture().Create<Uri>())
                {
                    Content = JsonContent.Create(new Fixture().Create<ClientCredentialsRequest>())
                });

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = JsonContent.Create(response)
                });

            // Act
            var result = await sut.AuthorizeAsync(new Fixture().Create<Uri>(), new Fixture().Create<ClientCredentialsRequest>(), default);

            // Assert
            Assert.Equal(response, result);
        }
    }
}
