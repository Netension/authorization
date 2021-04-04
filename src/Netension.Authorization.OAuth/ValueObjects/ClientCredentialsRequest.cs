using Netension.Core;
using System.Collections.Generic;

namespace Netension.Authorization.OAuth.ValueObjects
{
    public class ClientCredentialsRequest : ValueObject
    {
        public string GrantType { get; } = "client_credentials";

        public string ClientId { get; }

        public string ClientSecret { get; }

        public string Scope { get; }

        public ClientCredentialsRequest(string clientId, string clientSecret, string scope)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            Scope = scope;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new[] { GrantType, ClientId, ClientSecret, Scope };
        }
    }
}
