using System.Linq;
using System.Threading.Tasks;
using Domain.Orchestrators;
using CoreLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LocationsController : ControllerBase
	{
		private readonly IGetLocationsOrchestrator _orchestrator;
		private readonly IGetUsersByLocationOrchestrator _getUsersByLocationOrchestrator;

		public LocationsController(IGetLocationsOrchestrator orchestrator, IGetUsersByLocationOrchestrator getUsersByLocationOrchestrator)
		{
			_orchestrator = orchestrator;
			_getUsersByLocationOrchestrator = getUsersByLocationOrchestrator;
		}

		[HttpGet()]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Get()
		{
			var result = await _orchestrator.Get();

			if (result.Errors.Any())
			{
				return NotFound(result.Errors);
			}

			return Ok(result.Value);
		}

		/// <summary>
		/// Right now just returns a list of users for that location.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Get(int id)
		{
			var result = await _getUsersByLocationOrchestrator.Get(id);

			if (result.Errors.Any())
			{
				return NotFound(result.Errors);
			}

			return Ok(result.Value);
		}
	}
}
