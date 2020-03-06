using Domain.Orchestrators;
using CoreLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingSuitePlatform.Helpers;
using System.Threading.Tasks;

namespace ShoppingSuitePlatform.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ImpersonationController : ControllerBase
	{
		private readonly IConfiguration _config;
		private readonly IImpersonateOrchestrator _orchestrator;

		public ImpersonationController(IConfiguration config, IImpersonateOrchestrator orchestrator)
		{
			(_config, _orchestrator) = (config, orchestrator);
		}

		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Post([FromBody] int impersonatingUserId)
		{
			var result = await _orchestrator.GetImpersonateClaims(impersonatingUserId);

			if (result.Value is null)
			{
				return NotFound(result.Errors);
			}

			var jwtToken = new JwtTokenHelper(_config).GenerateJSONWebToken(result.Value);
			return Ok(new { token = jwtToken });
		}
	}
}