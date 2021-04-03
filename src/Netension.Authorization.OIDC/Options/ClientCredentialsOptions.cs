using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Netension.Authorization.OIDC.Options
{
    public class ClientCredentialsOptions
    {
        [Required]
        public Uri Authority { get; set; }
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string ClientSecret { get; set; }

        public IEnumerable<string> Scopes { get; set; }
    }
}
