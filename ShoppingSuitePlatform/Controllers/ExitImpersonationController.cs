using Domain.Orchestrators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingSuitePlatform.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingSuitePlatform.Controllers
{
	[Route("[controller]")]
    [ApiController]
    public class ExitImpersonationController : ControllerBase
    {
		private readonly IConfiguration _config;
		private readonly IImpersonateOrchestrator _orchestrator;

		public ExitImpersonationController(IConfiguration config, IImpersonateOrchestrator orchestrator)
		{
			(_config, _orchestrator) = (config, orchestrator);
		}

		[Authorize()]
		public async Task<ActionResult> Post()
		{
			var result = await _orchestrator.GetExitImpersonateClaims();

			if (result.Errors.Any())
			{
				return BadRequest(result.Errors);
			}

			var jwtToken = new JwtTokenHelper(_config).GenerateJSONWebToken(result.Value);
			return Ok(new { token = jwtToken });
		}
	}
}