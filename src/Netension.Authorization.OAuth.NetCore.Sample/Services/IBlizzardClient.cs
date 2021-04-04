using System.Threading;
using System.Threading.Tasks;

namespace Netension.Authorization.OAuth.NetCore.Sample.Services
{
    public interface IBlizzardClient
    {
        Task<string> GetActsAsync(CancellationToken cancellationToken);
    }
}