using Netension.Authorization.OAuth.Converters;
using Netension.Core;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netension.Authorization.OAuth.ValueObjects
{
    public class TokenResponse : ValueObject
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; }

        [JsonPropertyName("expires_in")]
        [JsonConverter(typeof(ExpiresInJsonConverter))]
        public TimeSpan ExpiresIn { get; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; }

        [JsonPropertyName("scope")]
        public string Scope { get; }

        public TokenResponse(string accessToken, string tokenType, TimeSpan expiresIn, string refreshToken, string scope)
        {
            AccessToken = accessToken;
            TokenType = tokenType;
            ExpiresIn = expiresIn;
            RefreshToken = refreshToken;
            Scope = scope;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new object[] { AccessToken.ToString(), TokenType, ExpiresIn, RefreshToken.ToString(), Scope };
        }
    }
}
