using Domain.Orchestrators;
using Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AccessListController : ControllerBase
	{
		private readonly IAccessListOrchestrator _orchestrator;

		public AccessListController(IAccessListOrchestrator orchestrator)
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

		[HttpGet("{id}")]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Get(int id)
		{
			var result = await _orchestrator.Get(id);

			if (result.Errors.Any())
			{
				return NotFound(result.Errors);
			}

			return Ok(result.Value);
		}
	}
}
