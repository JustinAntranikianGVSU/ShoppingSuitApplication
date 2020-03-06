using System.Threading.Tasks;
using CoreLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Orchestrators.Locations;
using ShoppingSuitePlatform.Controllers.BaseControllers;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LocationsController : AppControllerBase
	{
		private readonly IGetLocationsOrchestrator _getLocationsOrchestrator;
		private readonly IGetUsersByLocationOrchestrator _getUsersByLocationOrchestrator;

		public LocationsController(IGetLocationsOrchestrator getLocationsOrchestrator, IGetUsersByLocationOrchestrator getUsersByLocationOrchestrator)
		{
			(_getLocationsOrchestrator, _getUsersByLocationOrchestrator) = (getLocationsOrchestrator, getUsersByLocationOrchestrator);
		}

		[HttpGet()]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Get()
		{
			var result = await _getLocationsOrchestrator.Get();
			return NotFoundIfNotProcessed(result);
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
			return NotFoundIfNotProcessed(result);
		}
	}
}
