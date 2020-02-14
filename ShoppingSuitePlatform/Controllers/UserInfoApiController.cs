using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Orchestrators;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		public static List<User> Users = new List<User>();

		[HttpGet]
		public async Task<User> Get()
		{
			return new User();
		}

		[HttpPost]
		public async Task<ActionResult> Post([FromBody] User userBasicInfo)
		{
			var repo = new UserRepository(Users);
			var orchestrator = new CreateUserOrchestrator(repo);
			var result = await orchestrator.Create(userBasicInfo);

			if (result.Errors.Any())
			{
				return BadRequest(result.Errors);
			}

			return Ok(result.Value);
		}
	}
}
