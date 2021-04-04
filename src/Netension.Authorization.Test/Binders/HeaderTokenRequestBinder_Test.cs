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
    public class HeaderTokenRequestBinder_Test
    {
        private readonly ILogger<HeaderTokenRequestBinder> _logger;

        public HeaderTokenRequestBinder_Test(ITestOutputHelper outputHelper)
        {
            _logger = new LoggerFactory()
                        .AddXUnit(outputHelper)
                        .CreateLogger<HeaderTokenRequestBinder>();
        }

        private HeaderTokenRequestBinder CreateSUT()
        {
            return new HeaderTokenRequestBinder(_logger);
        }

        [Fact(DisplayName = "HeaderTokenRequestBinder - Bind - Set method to POST")]
        public void HeaderTokenRequestBinder_Bind_SetMethodToPost()
        {
            // Arrange
            var sut = CreateSUT();

            // Act
            var result = sut.Bind(new Fixture().Create<Uri>(), new Fixture().Create<ClientCredentialsRequest>());

            // Assert
            Assert.Equal(HttpMethod.Post, result.Method);
        }

        [Fact(DisplayName = "HeaderTokenRequestBinder - Bind - Set endpoint")]
        public void HeaderTokenRequestBinder_Bind_SetEndpoint()
        {
            // Arrange
            var sut = CreateSUT();
            var endpoint = new Fixture().Create<Uri>();

            // Act
            var result = sut.Bind(endpoint, new Fixture().Create<ClientCredentialsRequest>());

            // Assert
            Assert.Equal(endpoint, result.RequestUri);
        }

        [Fact(DisplayName = "HeaderTokenRequestBinder - Bind - Set authorization header")]
        public void HeaderTokenRequestBinder_Bind_SetAuthorizationHeader()
        {
            // Arrange
            var sut = CreateSUT();
            var request = new Fixture().Create<ClientCredentialsRequest>();

            // Act
            var result = sut.Bind(new Fixture().Create<Uri>(), request);

            // Assert
            var header = new BasicAuthenticationHeaderValue(request.ClientId, request.ClientSecret);
            Assert.Equal(header, result.Headers.Authorization);
        }

        [Fact(DisplayName = "HeaderTokenRequestBinder - Bind - Set grant type")]
        public async Task HeaderTokenRequestBinder_Bind_SetGrantType()
        {
            // Arrange
            var sut = CreateSUT();
            var request = new Fixture().Create<ClientCredentialsRequest>();

            // Act
            var result = sut.Bind(new Fixture().Create<Uri>(), request);

            // Assert
            Assert.Equal(await new FormUrlEncodedContent(new Dictionary<string, string> { { "grant_type", request.GrantType }, { "scopes", request.Scopes } }).ReadAsByteArrayAsync(), await result.Content.ReadAsByteArrayAsync());
        }
    }
}
