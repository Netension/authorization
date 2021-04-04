using AutoFixture;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Netension.Authorization.OAuth.Storages;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Netension.Authorization.Test.Storages
{
    public class TokenStorage_Test
    {
        private readonly ILogger<TokenStorage> _logger;
        private Mock<IDistributedCache> _distributeCacheMock;
        private string _key;

        public TokenStorage_Test(ITestOutputHelper outputHelper)
        {
            _logger = new LoggerFactory()
                        .AddXUnit(outputHelper)
                        .CreateLogger<TokenStorage>();
        }

        private TokenStorage CreateSUT()
        {
            _key = new Fixture().Create<string>();
            _distributeCacheMock = new Mock<IDistributedCache>();

            return new TokenStorage(_key, _distributeCacheMock.Object, _logger);
        }

        [Fact(DisplayName = "TokenStorage - StoreAccessToken - With expiration")]
        public async Task TokenStorage_StoreAccessToken_WithExpriation()
        {
            // Arrange
            var sut = CreateSUT();
            var token = new Fixture().Create<string>();
            var expiration = new Fixture().Create<TimeSpan>();

            // Act
            await sut.StoreAccessTokenAsync(token, expiration, default);

            // Assert
            _distributeCacheMock.Verify(dc => dc.SetAsync(It.Is<string>(k => k.Equals($"{_key}-access_token")), It.Is<byte[]>(v => v.SequenceEqual(Encoding.UTF8.GetBytes(token))), It.Is<DistributedCacheEntryOptions>(dceo => dceo.AbsoluteExpirationRelativeToNow.Equals(expiration)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "TokenStorage - StoreAccessTokenAsync - Without expiration")]
        public async Task TokenStorage_StoreAccessTokenAsync_WithoutExpiration()
        {
            // Arrange
            var sut = CreateSUT();
            var token = new Fixture().Create<string>();

            // Act
            await sut.StoreAccessTokenAsync(token, default);

            // Assert
            _distributeCacheMock.Verify(dc => dc.SetAsync(It.Is<string>(k => k.Equals($"{_key}-access_token")), It.Is<byte[]>(v => v.SequenceEqual(Encoding.UTF8.GetBytes(token))), It.Is<DistributedCacheEntryOptions>(dceo => !dceo.AbsoluteExpirationRelativeToNow.HasValue ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "TokenStorage - StoreAccessToken - Invalid token")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task TokenStorage_StoreAccessTokenAsync_InvalidToken(string token)
        {
            // Arrange
            var sut = CreateSUT();

            // Act
            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreAccessTokenAsync(token, default) );
        }

        [Fact(DisplayName = "TokenStorage - GetAccessTokenAsync - Token exists")]
        public async Task TokenStorage_GetAccessTokenAsync_TokenExists()
        {
            // Arrange
            var sut = CreateSUT();
            var token = new Fixture().Create<string>();

            _distributeCacheMock.Setup(dc => dc.GetAsync(It.Is<string>(k => k.Equals($"{_key}-access_token")), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Encoding.UTF8.GetBytes(token));

            // Act
            var result = await sut.GetAccessTokenAsync(default);

            // Assert
            Assert.Equal(token, result);
        }

        [Fact(DisplayName = "TokenStorage - GetAccessTokenAsync - Token does not exist")]
        public async Task TokenStorage_GetAccessTokenAsync_TokenDoesNotExists()
        {
            // Arrange
            var sut = CreateSUT();

            // Act
            var result = await sut.GetAccessTokenAsync(default);

            // Assert
            Assert.Null(result);
        }
    }
}
