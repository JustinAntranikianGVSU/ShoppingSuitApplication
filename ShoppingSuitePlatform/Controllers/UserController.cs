using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreLibrary;
using Domain.Orchestrators.Users;
using ShoppingSuitePlatform.Controllers.BaseControllers;
using Domain;
using Domain.Dtos;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : AppControllerBase
	{
		private readonly ICreateUserOrchestrator _createUserOrchestrator;
		private readonly IGetUserOrchestrator _getUserOrchestrator;
		private readonly IUpdateUserOrchestrator _updateUserOrchestrator;

		public UserController(ICreateUserOrchestrator createUserOrchestrator, IGetUserOrchestrator getUserOrchestrator, IUpdateUserOrchestrator updateUserOrchestrator)
		{
			(_createUserOrchestrator, _getUserOrchestrator, _updateUserOrchestrator) = (createUserOrchestrator, getUserOrchestrator, updateUserOrchestrator);
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
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Post([FromBody] UserDto user)
		{
			var result = await _createUserOrchestrator.Create(user);
			return BadRequestIfNotProcessed(result);
		}

		[HttpPut("{id}")]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Put(int id, [FromBody] UserUpdateDto userUpdateDto)
		{
			var result = await _updateUserOrchestrator.Update(id, userUpdateDto);
			return BadRequestIfNotProcessed(result);
		}
	}
}
