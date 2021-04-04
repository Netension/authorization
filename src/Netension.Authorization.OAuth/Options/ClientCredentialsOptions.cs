using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Netension.Authorization.OAuth.Options
{
    public class ClientCredentialsOptions
    {
        [Required]
        public Uri TokenEndpoint { get; set; }
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string ClientSecret { get; set; }

        public IEnumerable<string> Scopes { get; set; } = Enumerable.Empty<string>();
    }
}
