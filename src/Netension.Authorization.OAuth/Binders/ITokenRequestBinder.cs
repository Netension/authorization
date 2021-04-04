using Netension.Authorization.OAuth.ValueObjects;
using System;
using System.Net.Http;

namespace Netension.Authorization.OAuth.Binders
{
    public interface ITokenRequestBinder
    {
        HttpRequestMessage Bind(Uri tokenEndpoint, ClientCredentialsRequest request);
    }
}
