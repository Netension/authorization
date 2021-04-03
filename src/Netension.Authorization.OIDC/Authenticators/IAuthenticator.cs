using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OIDC.Authenticators
{
    public interface IAuthenticator
    {
        Task<string> AuthenticateAsync(CancellationToken cancellationToken);
    }
}
