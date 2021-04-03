using Netension.Core;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netension.Authorization.OIDC.ValueObjects
{
    public class Configuration : ValueObject
    {
        [JsonPropertyName("issuer")]
        public string Issuer { get; }
        [JsonPropertyName("authorization_endpoint")]
        public Uri AuthorizationEndpoint { get; }
        [JsonPropertyName("token_endpoint")]
        public Uri TokenEndpoint { get; }
        [JsonPropertyName("userinfo_endpoint")]
        public Uri UserInfoEndpoint { get; }

        public Configuration(string issuer, Uri authorizationEndpoint, Uri tokenEndpoint, Uri userInfoEndpoint)
        {
            Issuer = issuer;
            AuthorizationEndpoint = authorizationEndpoint;
            TokenEndpoint = tokenEndpoint;
            UserInfoEndpoint = userInfoEndpoint;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new object[] { Issuer, AuthorizationEndpoint, TokenEndpoint, UserInfoEndpoint };
        }
    }
}
