using Microsoft.AspNetCore.Mvc;
using Netension.Authorization.OAuth.Authenticators;
using Netension.Authorization.OAuth.NetCore.Sample.Services;
using System;
using System.Threading.Tasks;

namespace Netension.Authorization.OAuth.NetCore.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly Func<string, IAuthenticator> _authenticatorFactory;
        private readonly IBlizzardClient _blizzardClient;

        public SampleController(Func<string, IAuthenticator> authenticatorFactory, IBlizzardClient blizzardClient)
        {
            _authenticatorFactory = authenticatorFactory;
            _blizzardClient = blizzardClient;
        }

        [HttpGet("{scheme:alpha}")]
        public async Task<IActionResult> GetAsync(string scheme)
        {
            var token = await _authenticatorFactory(scheme).AuthenticateAsync(default);
            return Ok(token);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _blizzardClient.GetActsAsync(default));
        }
    }
}
