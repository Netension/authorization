using Microsoft.Extensions.Logging;
using Netension.Authorization.OAuth.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Netension.Authorization.OAuth.Binders
{
    public class BodyTokenRequestBinder : ITokenRequestBinder
    {
        private readonly ILogger<BodyTokenRequestBinder> _logger;

        public BodyTokenRequestBinder(ILogger<BodyTokenRequestBinder> logger)
        {
            _logger = logger;
        }

        public HttpRequestMessage Bind(Uri tokenEndpoint, ClientCredentialsRequest request)
        {
            _logger.LogDebug("Use {type}", nameof(BodyTokenRequestBinder));

            var content = new Dictionary<string, string>
                    {
                        { "grant_type", request.GrantType },
                        { "client_id", request.ClientId },
                        { "client_secret", request.ClientSecret }
                    };

            if (request.Scope.Any()) content.Add("scopes", request.Scope);

            return new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
            {
                Content = new FormUrlEncodedContent(content)
            };
        }
    }
}
