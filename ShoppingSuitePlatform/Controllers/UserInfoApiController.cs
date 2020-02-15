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

		public UserController(ICreateUserOrchestrator createUserOrchestrator)
		{
			_createUserOrchestrator = createUserOrchestrator;
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
