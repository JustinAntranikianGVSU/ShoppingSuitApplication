using Domain.Orchestrators;
using CoreLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using ShoppingSuitePlatform.Controllers.BaseControllers;

namespace ShoppingSuitePlatform.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ImpersonationController : TokenGeneratorControllerBase
	{
		private readonly IImpersonateOrchestrator _orchestrator;

		public ImpersonationController(IImpersonateOrchestrator orchestrator, IConfiguration config) : base(config)
		{
			_orchestrator = orchestrator;
		}

		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Post([FromBody] int impersonatingUserId)
		{
			var result = await _orchestrator.GetImpersonateClaims(impersonatingUserId);
			return GetTokenResult(result);
		}
	}
}