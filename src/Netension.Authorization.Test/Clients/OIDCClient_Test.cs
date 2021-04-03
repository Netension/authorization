using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Netension.Authorization.OIDC.Clients;
using Netension.Authorization.OIDC.ValueObjects;
using System;
using System.IdentityModel.Tokens.Jwt;
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
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(hrm => hrm.ValidateDiscovery(BASE_ADRESS)), ItExpr.IsAny<CancellationToken>());
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

        [Fact(DisplayName = "OIDCClient - AuthorizeAsync - Get token")]
        public async Task OIDCClient_AuthorizeAsync_GetToken()
        {
            // Arrange
            var accessToken = new JwtSecurityToken();
            var refreshToken = new JwtSecurityToken();
            var configuration = new Fixture().Create<Configuration>();
            var sut = CreateSUT(configuration);
            var request = new Fixture().Create<ClientCredentialsRequest>();

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = JsonContent.Create(new TokenResponse(accessToken,
                        new Fixture().Create<string>(), TimeSpan.FromMinutes(5), refreshToken,
                        new Fixture().Create<string>()))
                });

            // Act
            await sut.AuthorizeAsync(request, configuration, default);

            // Assert
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(hrm => hrm.ValidateAuthorize(configuration.TokenEndpoint, request)), ItExpr.IsAny<CancellationToken>());
        }

        [Fact(DisplayName = "OIDCClient - AuthorizeAsync - Parse token")]
        public async Task OIDCClient_AuthorizeAsync_ParseToken()
        {
            // Arrange
            var response = new TokenResponse(new JwtSecurityToken(), new Fixture().Create<string>(), TimeSpan.FromMinutes(5), new JwtSecurityToken(), new Fixture().Create<string>());
            var configuration = new Fixture().Create<Configuration>();
            var sut = CreateSUT(configuration);

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = JsonContent.Create(response)
                });

            // Act
            var result =  await sut.AuthorizeAsync(new Fixture().Create<ClientCredentialsRequest>(), configuration, default);

            // Assert
            Assert.Equal(response, result);
        }

        [Fact(DisplayName = "OIDCClient - RefreshAsync - Refresh token")]
        public async Task OIDCClient_RefreshAsync_RefreshToken()
        {
            // Arrange
            var accessToken = new JwtSecurityToken();
            var refreshToken = new JwtSecurityToken();
            var configuration = new Fixture().Create<Configuration>();
            var sut = CreateSUT(configuration);
            var request = new RefreshTokenRequest(new JwtSecurityToken(), new Fixture().Create<string>());

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = JsonContent.Create(new TokenResponse(accessToken,
                        new Fixture().Create<string>(), TimeSpan.FromMinutes(5), refreshToken,
                        new Fixture().Create<string>()))
                });

            // Act
            await sut.RefreshAsync(request, configuration, default);

            // Assert
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(hrm => hrm.ValidateRefresh(configuration.TokenEndpoint, request)), ItExpr.IsAny<CancellationToken>());
        }

        [Fact(DisplayName = "OIDCClient - RefreshAsync - Parse token")]
        public async Task OIDCClient_RefreshAsync_ParseToken()
        {
            // Arrange
            var response = new TokenResponse(new JwtSecurityToken(), new Fixture().Create<string>(), TimeSpan.FromMinutes(5), new JwtSecurityToken(), new Fixture().Create<string>());
            var configuration = new Fixture().Create<Configuration>();
            var sut = CreateSUT(configuration);

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = JsonContent.Create(response)
                });

            // Act
            var result = await sut.RefreshAsync(new RefreshTokenRequest(new JwtSecurityToken(), new Fixture().Create<string>()), configuration, default);

            // Assert
            Assert.Equal(response, result);
        }
    }

    public static class TestExtensions
    {
        public static bool ValidateDiscovery(this HttpRequestMessage request, Uri baseAddress)
        {
            return request.RequestUri.Authority.Equals(baseAddress.Authority) &&
                request.RequestUri.AbsolutePath.Equals(OIDCDefaults.DiscoveryPath) &&
                request.RequestUri.Scheme.Equals(baseAddress.Scheme) &&
                request.Method.Method.Equals(HttpMethod.Get.Method);
        }

        public static bool ValidateAuthorize(this HttpRequestMessage request, Uri address, ClientCredentialsRequest clientCredentialsRequest)
        {
            return request.RequestUri.Authority.Equals(address.Authority) &&
                request.RequestUri.AbsolutePath.Equals(address.AbsolutePath) &&
                request.RequestUri.Scheme.Equals(address.Scheme) &&
                request.Method.Method.Equals(HttpMethod.Post.Method) &&
                request.Content.ReadFromJsonAsync<ClientCredentialsRequest>().Result.Equals(clientCredentialsRequest);
        }

        public static bool ValidateRefresh(this HttpRequestMessage request, Uri address, RefreshTokenRequest refreshTokenRequest)
        {
            return request.RequestUri.Authority.Equals(address.Authority) &&
                request.RequestUri.AbsolutePath.Equals(address.AbsolutePath) &&
                request.RequestUri.Scheme.Equals(address.Scheme) &&
                request.Method.Method.Equals(HttpMethod.Post.Method) &&
                request.Content.ReadFromJsonAsync<RefreshTokenRequest>().Result.Equals(refreshTokenRequest);
        }
    }
}
