using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OAuth.Authenticators
{
    public interface IAuthenticator
    {
        Task<string> AuthenticateAsync(CancellationToken cancellationToken);
    }
}
