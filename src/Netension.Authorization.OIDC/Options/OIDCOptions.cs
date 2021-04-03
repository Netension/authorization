using System;
using System.ComponentModel.DataAnnotations;

namespace Netension.Authorization.OIDC.Options
{
    public class OIDCOptions
    {
        [Required]
        public Uri Authority { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }

        public OIDCOptions(Uri authority, string clientId, string clientSecret)
        {
            Authority = authority;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }
}
