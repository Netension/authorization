using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Netension.Authorization.OAuth.Authenticators;
using Netension.Authorization.OAuth.Clients;
using Netension.Authorization.OAuth.Options;
using Netension.Authorization.OAuth.Storages;
using Netension.Authorization.OAuth.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Netension.Authorization.Test.Authenticators
{
    public class ClientCredentialsAuthenticator_Test
    {
        private readonly ILogger<ClientCredentialsAuthenticator> _logger;
        private ClientCredentialsOptions _options;
        private Mock<IOAuthClient> _oauthClientMock;
        private Mock<ITokenStorage> _tokenStorageMock;

        public ClientCredentialsAuthenticator_Test(ITestOutputHelper outputHelper)
        {
            _logger = new LoggerFactory()
                        .AddXUnit(outputHelper)
                        .CreateLogger<ClientCredentialsAuthenticator>();
        }

        private ClientCredentialsAuthenticator CreateSUT()
        {
            _options = new ClientCredentialsOptions();
            _oauthClientMock = new Mock<IOAuthClient>();
            _tokenStorageMock = new Mock<ITokenStorage>();

            return new ClientCredentialsAuthenticator(_options, _oauthClientMock.Object, _tokenStorageMock.Object, _logger);
        }

        [Fact(DisplayName = "ClientCredentialsAuthenticator - AuthenticateAsync - Get token from storage")]
        public async Task ClientCredentialsAuthenticator_AuthenticateAsync_GetTokenFromStorage()
        {
            // Arrange
            var sut = CreateSUT();

            _oauthClientMock.Setup(oc => oc.CallTokenEndpointAsync(It.IsAny<Uri>(), It.IsAny<ClientCredentialsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Fixture().Create<TokenResponse>());

            // Act
            await sut.AuthenticateAsync(default);

            // Assert
            _tokenStorageMock.Verify(ts => ts.GetAccessTokenAsync(It.IsAny<CancellationToken>()));
        }

        [Fact(DisplayName = "ClientCredentialsAuthenticator - AuthenticateAsync - Get token from OAuthClient")]
        public async Task ClientCredentialsAuthenticator_AuthenticateAsync_GetTokenFromOAuthClient()
        {
            // Arrange
            var sut = CreateSUT();
            _options.TokenEndpoint = new Fixture().Create<Uri>();
            var request = new ClientCredentialsRequest(_options.ClientId, _options.ClientSecret, string.Join(' ', _options.Scopes));

            _oauthClientMock.Setup(oc => oc.CallTokenEndpointAsync(It.IsAny<Uri>(), It.IsAny<ClientCredentialsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Fixture().Create<TokenResponse>());

            // Act
            await sut.AuthenticateAsync(default);

            // Assert
            _oauthClientMock.Verify(oc => oc.CallTokenEndpointAsync(It.Is<Uri>(te => te.Equals(_options.TokenEndpoint)), It.Is<ClientCredentialsRequest>(ccr => ccr.Equals(request)), It.IsAny<CancellationToken>()));
        }

        [Fact(DisplayName = "ClientCredentialsAuthenticator - AuthenticateAsync - Store the token")]
        public async Task ClientCredentialsAuthenticator_AuthenticateAsync_StoreTheToken()
        {
            // Arrange
            var sut = CreateSUT();
            var response = new Fixture().Create<TokenResponse>();

            _oauthClientMock.Setup(oc => oc.CallTokenEndpointAsync(It.IsAny<Uri>(), It.IsAny<ClientCredentialsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            await sut.AuthenticateAsync(default);

            // Assert
            _tokenStorageMock.Verify(ts => ts.StoreAccessTokenAsync(It.Is<string>(t => t.Equals(response.AccessToken)), It.Is<TimeSpan>(e => e.Equals(response.ExpiresIn)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "ClientCredentialsAuthenticator - AuthenticateAsync - Use stored token")]
        public async Task ClientCredentialsAuthenticator_AuthenticateAsync_UseStoredToken()
        {
            // Arrange
            var sut = CreateSUT();

            _tokenStorageMock.Setup(ts => ts.GetAccessTokenAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Fixture().Create<string>());

            // Act
           await sut.AuthenticateAsync(default);

            // Assert
            _oauthClientMock.Verify(oc => oc.CallTokenEndpointAsync(It.IsAny<Uri>(), It.IsAny<ClientCredentialsRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
