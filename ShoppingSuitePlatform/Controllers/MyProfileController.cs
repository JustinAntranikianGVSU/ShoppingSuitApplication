using System.Threading.Tasks;
using Domain.Orchestrators;
using CoreLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingSuitePlatform.Controllers.BaseControllers;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MyProfileController : AppControllerBase
	{
		private readonly IMyProfileOrchestrator _orchestrator;

		public MyProfileController(IMyProfileOrchestrator orchestrator)
		{
			_orchestrator = orchestrator;
		}

		[HttpGet()]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Get()
		{
			var result = await _orchestrator.Get();
			return NotFoundIfNotProcessed(result);
		}
	}
}
