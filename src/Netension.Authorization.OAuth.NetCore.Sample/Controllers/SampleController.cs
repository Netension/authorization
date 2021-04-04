using Microsoft.AspNetCore.Mvc;
using Netension.Authorization.OAuth.Authenticators;
using System;
using System.Threading.Tasks;

namespace Netension.Authorization.OAuth.NetCore.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly Func<string, IAuthenticator> _authenticatorFactory;

        public SampleController(Func<string, IAuthenticator> authenticatorFactory)
        {
            _authenticatorFactory = authenticatorFactory;
        }

        [HttpGet("{scheme:alpha}")]
        public async Task<IActionResult> GetAsync(string scheme)
        {
            var token = await _authenticatorFactory(scheme).AuthenticateAsync(default);
            return Ok(token);
        }
    }
}
