using Netension.Core;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netension.Authorization.OIDC.ValueObjects
{
    public class ClientCredentialsRequest : ValueObject
    {
        [JsonPropertyName("grant_type")]
        public string GrantType { get; } = "client_credentials";

        [JsonPropertyName("client_id")]
        [JsonIgnore]
        public string ClientId { get; }

        [JsonPropertyName("client_secret")]
        [JsonIgnore]
        public string ClientSecret { get; }

        [JsonPropertyName("scope")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Scope { get; }

        public ClientCredentialsRequest(string clientId, string clientSecret, string scope)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            Scope = string.IsNullOrWhiteSpace(scope) ? null : scope;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new[] { GrantType, ClientId, ClientSecret, Scope };
        }
    }
}
