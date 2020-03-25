using CoreLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingSuitePlatform.Controllers.BaseControllers;
using Domain.Orchestrators;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RolesController : AppControllerBase
	{
		private readonly IRolesOrchestrator _rolesOrchestrator;

		public RolesController(IRolesOrchestrator rolesOrchestrator)
		{
			_rolesOrchestrator = rolesOrchestrator;
		}

		[HttpGet()]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public ActionResult Get()
		{
			var result =_rolesOrchestrator.GetRoles();
			return NotFoundIfNotProcessed(result);
		}
	}
}
