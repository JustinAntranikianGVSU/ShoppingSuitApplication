using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Orchestrators;
using Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingSuitePlatform.MiddleWare;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly ICreateUserOrchestrator _createUserOrchestrator;

		private readonly IGetUserOrchestrator _getUserOrchestrator;

		private readonly JwtUserContext _jwtUserContext;

		public UserController
		(
			ICreateUserOrchestrator createUserOrchestrator,
			IGetUserOrchestrator getUserOrchestrator,
			JwtUserContext jwtUserContext
		)
		{
			_createUserOrchestrator = createUserOrchestrator;
			_getUserOrchestrator = getUserOrchestrator;
			_jwtUserContext = jwtUserContext;
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

			if (result.Errors.Any())
			{
				return NotFound(result.Errors);
			}

			return Ok(result.Value);
		}

		[HttpPost]
		public async Task<ActionResult> Post([FromBody] User user)
		{
			var result = await _createUserOrchestrator.Create(user);

			if (result.Errors.Any())
			{
				return BadRequest(result.Errors);
			}

			return Ok(result.Value);
		}
	}
}
