using System.Threading.Tasks;
using CoreLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingSuitePlatform.Controllers.BaseControllers;
using Domain.Orchestrators;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LocationsController : AppControllerBase
	{
		private readonly IGetLocationsOrchestrator _getLocationsOrchestrator;

		public LocationsController(IGetLocationsOrchestrator getLocationsOrchestrator)
		{
			_getLocationsOrchestrator = getLocationsOrchestrator;
		}

		[HttpGet()]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Get()
		{
			var result = await _getLocationsOrchestrator.GetAll();
			return NotFoundIfNotProcessed(result);
		}
	}
}
