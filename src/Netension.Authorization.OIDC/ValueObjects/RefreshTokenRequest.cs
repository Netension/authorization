using Netension.Authorization.OIDC.Converters;
using Netension.Core;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

namespace Netension.Authorization.OIDC.ValueObjects
{
    public class RefreshTokenRequest : ValueObject
    {
        [JsonPropertyName("grant_type")]
        public string GrantType { get; } = "refresh_token";

        [JsonPropertyName("refresh_token")]
        [JsonConverter(typeof(JwtSecurityTokenJsonConverter))]
        public JwtSecurityToken RefreshToken { get; }

        [JsonPropertyName("scope")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Scope { get; }

        public RefreshTokenRequest(JwtSecurityToken refreshToken, string scope)
        {
            RefreshToken = refreshToken;
            Scope = scope;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new object[] { GrantType, RefreshToken.ToString(), Scope };
        }
    }
}
