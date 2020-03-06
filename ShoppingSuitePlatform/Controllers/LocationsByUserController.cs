using System.Threading.Tasks;
using CoreLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Orchestrators.Users;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LocationsByUserController : ControllerBase
	{
		private readonly IGetUserOrchestrator _orchestrator;

		public LocationsByUserController(IGetUserOrchestrator orchestrator)
		{
			_orchestrator = orchestrator;
		}

		[HttpGet()]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Get()
		{
			var result = await _orchestrator.GetLocationsForLoggedInUser();
			return Ok(result.Value);
		}

		[HttpGet("{id}")]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Get(int id)
		{
			var result = await _orchestrator.GetLocations(id);
			return Ok(result.Value);
		}
	}
}
