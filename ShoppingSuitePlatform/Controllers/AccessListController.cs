using Domain.Orchestrators;
using CoreLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ShoppingSuitePlatform.Controllers.BaseControllers;
using Domain.Dtos;
using Domain.Orchestrators.Users;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AccessListController : AppControllerBase
	{
		private readonly IGetAccessListOrchestrator _getAccessListOrchestrator;
		private readonly IUpdateAccessListOrchestrator _updateAccessListOrchestrator;

		public AccessListController(IGetAccessListOrchestrator getAccessListOrchestrator, IUpdateAccessListOrchestrator updateAccessListOrchestrator)
		{
			_getAccessListOrchestrator = getAccessListOrchestrator;
			_updateAccessListOrchestrator = updateAccessListOrchestrator;
		}

		[HttpGet()]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Get()
		{
			var result = await _getAccessListOrchestrator.GetAll();
			return NotFoundIfNotProcessed(result);
		}

		[HttpGet("{id}")]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Get(int id)
		{
			var result = await _getAccessListOrchestrator.Get(id);
			return NotFoundIfNotProcessed(result);
		}

		[HttpPut("{id}")]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Put(int id, [FromBody] AccessListUpdateDto accessListUpdateDto)
		{
			var result = await _updateAccessListOrchestrator.Update(id, accessListUpdateDto);
			return BadRequestIfNotProcessed(result);
		}
	}
}
