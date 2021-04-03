using Microsoft.AspNetCore.Mvc;
using Netension.Authorization.OIDC.Authenticators;
using System.Threading.Tasks;

namespace Netension.Authorization.OIDC.NetCore.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IAuthenticator _authenticator;

        public SampleController(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var token =  await _authenticator.AuthenticateAsync(default);
            return Ok(token);
        }
    }
}
