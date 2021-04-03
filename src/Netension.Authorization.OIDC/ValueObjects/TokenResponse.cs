using Netension.Authorization.OIDC.Converters;
using Netension.Core;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

namespace Netension.Authorization.OIDC.ValueObjects
{
    public class TokenResponse : ValueObject
    {
        [JsonPropertyName("access_token")]
        [JsonConverter(typeof(JwtSecurityTokenJsonConverter))]
        public JwtSecurityToken AccessToken { get; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; }

        [JsonPropertyName("expires_in")]
        [JsonConverter(typeof(ExpiresInJsonConverter))]
        public TimeSpan ExpiresIn { get; }

        [JsonPropertyName("refresh_token")]
        [JsonConverter(typeof(JwtSecurityTokenJsonConverter))]
        public JwtSecurityToken RefreshToken { get; }

        [JsonPropertyName("scope")]
        public string Scope { get; }

        public TokenResponse(JwtSecurityToken accessToken, string tokenType, TimeSpan expiresIn, JwtSecurityToken refreshToken, string scope)
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
