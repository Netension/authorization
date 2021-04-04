using AutoFixture;
using Microsoft.Extensions.Logging;
using Netension.Authorization.OAuth.Storages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Netension.Authorization.Test.Storages
{
    public class MemoryTokenStorage_Test
    {
        private readonly ILogger<MemoryTokenStorage> _logger;

        public MemoryTokenStorage_Test(ITestOutputHelper outputHelper)
        {
            _logger = new LoggerFactory()
                        .AddXUnit(outputHelper)
                        .CreateLogger<MemoryTokenStorage>();
        }

        private MemoryTokenStorage CreateSUT()
        {
            return new MemoryTokenStorage();
        }

        [Fact(DisplayName = "MemoryTokenStorage - StoreAccessTokenAsync - Store token")]
        public async Task MemoryTokenStorage_StoreAccessTokenAsync_StoreToken()
        {
            // Arrange
            var sut = CreateSUT();
            var token = new Fixture().Create<string>();

            // Act
            await sut.StoreAccessTokenAsync(token, default);

            // Assert
            Assert.Equal(token, await sut.GetAccessTokenAsync(default));
        }

        [Fact(DisplayName = "MemoryTokenStorage - StoreAccessTokenAsync - Expire token")]
        public async Task MemoryTokenStorage_StoreAccessTokenAsync_ExpireToken()
        {
            // Arrange
            var sut = CreateSUT();

            // Act
            await sut.StoreAccessTokenAsync(new Fixture().Create<string>(), TimeSpan.FromSeconds(1), default);

            await Task.Delay(TimeSpan.FromSeconds(2));

            // Assert
            Assert.Null(await sut.GetAccessTokenAsync(default));
        }
    }
}
