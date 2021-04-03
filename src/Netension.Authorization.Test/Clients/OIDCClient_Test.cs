using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Netension.Authorization.OIDC.Clients;
using Netension.Authorization.OIDC.ValueObjects;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Netension.Authorization.Test.Clients
{
    public class OIDCClient_Test
    {
        private readonly ILogger<OIDCClient> _logger;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;

        private readonly Uri BASE_ADRESS = new Uri("http://test-authority");

        public OIDCClient_Test(ITestOutputHelper outputHelper)
        {
            _logger = new LoggerFactory()
                        .AddXUnit(outputHelper)
                        .CreateLogger<OIDCClient>();
        }

        private OIDCClient CreateSUT(Configuration configuration)
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(configuration)
                });

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = BASE_ADRESS
            };

            return new OIDCClient(httpClient, _logger);
        }

        [Fact(DisplayName = "OIDCClient - DiscoverAsync - CallDiscoveryEndpoint")]
        public async Task OIDCClient_DiscoverAsync_CallDiscoveryEndpoint()
        {
            // Arrange
            var sut = CreateSUT(new Fixture().Create<Configuration>());

            // Act
            await sut.DiscoverAsync(default);

            // Assert
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(hrm => hrm.Validate(BASE_ADRESS)), ItExpr.IsAny<CancellationToken>());
        }

        [Fact(DisplayName = "OIDCClient - DiscoverAsync - Get configuration")]
        public async Task OIDCClient_DiscoverAsync_GetConfiguration()
        {
            // Arrange
            var configuration = new Fixture()
                                    .Create<Configuration>();
            var sut = CreateSUT(configuration);

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(configuration)
                });

            // Act
            var result = await sut.DiscoverAsync(default);

            // Assert
            Assert.Equal(configuration, result);
        }
    }

    public static class TestExtensions
    {
        public static bool Validate(this HttpRequestMessage request, Uri baseAddress)
        {
            return request.RequestUri.Authority.Equals(baseAddress.Authority) &&
                request.RequestUri.AbsolutePath.Equals(OIDCDefaults.DiscoveryPath) &&
                request.RequestUri.Scheme.Equals(baseAddress.Scheme);
        }
    }
}
