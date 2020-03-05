using System.Linq;
using System.Threading.Tasks;
using Domain.Orchestrators;
using Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LocationsController : ControllerBase
	{
		private readonly IGetLocationsOrchestrator _orchestrator;

		public LocationsController(IGetLocationsOrchestrator orchestrator)
		{
			_orchestrator = orchestrator;
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
			var result = await _orchestrator.GetUsersByLocation(id);

			if (result.Errors.Any())
			{
				return NotFound(result.Errors);
			}

			return Ok(result.Value);
		}
	}
}
