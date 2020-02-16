using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Orchestrators;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly ICreateUserOrchestrator _createUserOrchestrator;

		private readonly IGetUserOrchestrator _getUserOrchestrator;

		public UserController(ICreateUserOrchestrator createUserOrchestrator, IGetUserOrchestrator getUserOrchestrator)
		{
			_createUserOrchestrator = createUserOrchestrator;
			_getUserOrchestrator = getUserOrchestrator;
		}

		[HttpGet]
		public async Task<ActionResult> GetAll()
		{
			var result = await _getUserOrchestrator.GetAll();
			return Ok(result.Value);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult> Get(int id)
		{
			var result = await _getUserOrchestrator.Get(id);

			if (result.Errors.Any())
			{
				return BadRequest(result.Errors);
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
