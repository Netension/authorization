using Netension.Core;
using System;
using System.Collections.Generic;

namespace Netension.Authorization.OIDC.ValueObjects
{
    public class Configuration : ValueObject
    {
        public string Issuer { get; }
        public Uri AuthorizationEndpoint { get; }
        public Uri TokenEndpoint { get; }
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
