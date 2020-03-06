using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreLibrary;
using Domain.Orchestrators.Users;
using ShoppingSuitePlatform.Controllers.BaseControllers;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : AppControllerBase
	{
		private readonly ICreateUserOrchestrator _createUserOrchestrator;
		private readonly IGetUserOrchestrator _getUserOrchestrator;

		public UserController(ICreateUserOrchestrator createUserOrchestrator, IGetUserOrchestrator getUserOrchestrator)
		{
			(_createUserOrchestrator, _getUserOrchestrator) = (createUserOrchestrator, getUserOrchestrator);
		}

		[HttpGet]
		public async Task<ActionResult> GetAll()
		{
			var result = await _getUserOrchestrator.GetAll();
			return Ok(result.Value);
		}

		[HttpGet("{id}")]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Get(int id)
		{
			var result = await _getUserOrchestrator.Get(id);
			return NotFoundIfNotProcessed(result);
		}

		[HttpPost]
		public async Task<ActionResult> Post([FromBody] UserDto user)
		{
			var result = await _createUserOrchestrator.Create(user);
			return BadRequestIfNotProcessed(result);
		}
	}
}
