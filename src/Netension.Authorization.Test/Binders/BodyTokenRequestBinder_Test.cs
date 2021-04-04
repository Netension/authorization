using AutoFixture;
using Microsoft.Extensions.Logging;
using Netension.Authorization.OAuth.Binders;
using Netension.Authorization.OAuth.ValueObjects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Netension.Authorization.Test.Binders
{
    public class BodyTokenRequestBinder_Test
    {
        private readonly ILogger<BodyTokenRequestBinder> _logger;

        public BodyTokenRequestBinder_Test(ITestOutputHelper outputHelper)
        {
            _logger = new LoggerFactory()
                        .AddXUnit(outputHelper)
                        .CreateLogger<BodyTokenRequestBinder>();
        }

        private BodyTokenRequestBinder CreateSUT()
        {
            return new BodyTokenRequestBinder(_logger);
        }

        [Fact(DisplayName = "BodyTokenRequestBinder - Bind - Set method to POST")]
        public void BodyTokenRequestBinder_Bind_SetMethodToPost()
        {
            // Arrange
            var sut = CreateSUT();

            // Act
            var result = sut.Bind(new Fixture().Create<Uri>(), new Fixture().Create<ClientCredentialsRequest>());

            // Assert
            Assert.Equal(HttpMethod.Post, result.Method);
        }

        [Fact(DisplayName = "BodyTokenRequestBinder - Bind - Set endpoint")]
        public void BodyTokenRequestBinder_Bind_SetEndpoint()
        {
            // Arrange
            var sut = CreateSUT();
            var endpoint = new Fixture().Create<Uri>();

            // Act
            var result = sut.Bind(endpoint, new Fixture().Create<ClientCredentialsRequest>());

            // Assert
            Assert.Equal(endpoint, result.RequestUri);
        }

        [Fact(DisplayName = "BodyTokenRequestBinder - Bind - Set grant type")]
        public async Task BodyTokenRequestBinder_Bind_SetGrantType()
        {
            // Arrange
            var sut = CreateSUT();
            var request = new Fixture().Create<ClientCredentialsRequest>();

            // Act
            var result = sut.Bind(new Fixture().Create<Uri>(), request);

            // Assert
            var content = new FormUrlEncodedContent(new Dictionary<string, string> 
            {
                { "grant_type", request.GrantType },
                { "client_id", request.ClientId },
                { "client_secret", request.ClientSecret },
                { "scope", request.Scope }
            });
            Assert.Equal(await content.ReadAsByteArrayAsync(), await result.Content.ReadAsByteArrayAsync());
        }
    }
}
