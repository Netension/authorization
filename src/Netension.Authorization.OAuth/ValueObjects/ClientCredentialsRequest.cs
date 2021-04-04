using Netension.Core;
using System.Collections.Generic;

namespace Netension.Authorization.OAuth.ValueObjects
{
    public class ClientCredentialsRequest : ValueObject
    {
        public string GrantType { get; } = "client_credentials";

        public string ClientId { get; }

        public string ClientSecret { get; }

        public string Scopes { get; }

        public ClientCredentialsRequest(string clientId, string clientSecret, string scopes)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            Scopes = scopes;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new[] { GrantType, ClientId, ClientSecret, Scopes };
        }
    }
}
