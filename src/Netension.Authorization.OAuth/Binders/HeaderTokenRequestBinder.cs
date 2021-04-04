using Microsoft.Extensions.Logging;
using Netension.Authorization.OAuth.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Netension.Authorization.OAuth.Binders
{
    public class HeaderTokenRequestBinder : ITokenRequestBinder
    {
        private readonly ILogger<HeaderTokenRequestBinder> _logger;

        public HeaderTokenRequestBinder(ILogger<HeaderTokenRequestBinder> logger)
        {
            _logger = logger;
        }

        public HttpRequestMessage Bind(Uri tokenEndpoint, ClientCredentialsRequest request)
        {
            _logger.LogDebug("Use {type}", nameof(HeaderTokenRequestBinder));

            var result = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
            result.Headers.Authorization = new BasicAuthenticationHeaderValue(request.ClientId, request.ClientSecret);

            var content = new Dictionary<string, string> { { "grant_type", request.GrantType } };
            if (request.Scope.Any()) content.Add("scopes", request.Scope);

            result.Content = new FormUrlEncodedContent(content);

            return result;
        }
    }
}
