using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain;
using Domain.Orchestrators;
using Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly ICreateUserOrchestrator _createUserOrchestrator;

		private readonly IGetUserOrchestrator _getUserOrchestrator;

		//private readonly SignInManager<IdentityUser> _signInManager;

		public UserController
		(
			ICreateUserOrchestrator createUserOrchestrator,
			IGetUserOrchestrator getUserOrchestrator,
			SignInManager<IdentityUser> signInManager
		)
		{
			_createUserOrchestrator = createUserOrchestrator;
			_getUserOrchestrator = getUserOrchestrator;
			//_signInManager = signInManager;
		}

		[HttpGet]
		public async Task<ActionResult> GetAll()
		{
			//var claims = new List<Claim>
			//{
			//	new Claim(ClaimTypes.Role, RoleLookup.TrainingUserRoleGuid.ToString())
			//};

			//var identityUser = new IdentityUser();

			//await _signInManager.SignInWithClaimsAsync(identityUser, false, claims.ToArray());

			var result = await _getUserOrchestrator.GetAll();
			return Ok(result.Value);
		}

		[HttpGet("{id}")]
		[Authorize(Policy = "ViewEmployee")]
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
