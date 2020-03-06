using Domain.Orchestrators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingSuitePlatform.Controllers.BaseControllers;
using System.Threading.Tasks;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ExitImpersonationController : TokenGeneratorControllerBase
	{
		private readonly IImpersonateOrchestrator _orchestrator;

		public ExitImpersonationController(IImpersonateOrchestrator orchestrator, IConfiguration config) : base(config)
		{
			_orchestrator = orchestrator;
		}

		[Authorize()]
		public async Task<ActionResult> Post()
		{
			var result = await _orchestrator.GetExitImpersonateClaims();
			return GetTokenResult(result);
		}
	}
}