using Domain.Dtos;
using Domain.Orchestrators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LoginController : ControllerBase
	{
		private readonly ILoginOrchestrator _loginOrchestrator;

		private readonly SignInManager<IdentityUser> _signInManager;
		public LoginController(ILoginOrchestrator loginOrchestrator, SignInManager<IdentityUser> signInManager)
		{
			(_loginOrchestrator, _signInManager) = (loginOrchestrator, signInManager);
		}

		[HttpPost]
		public async Task<ActionResult> Post([FromBody] LoginRequestDto loginRequestDto)
		{
			var result = await _loginOrchestrator.GetLoginReponse(loginRequestDto);

			if (result.Errors.Any())
			{
				return BadRequest(result.Errors);
			}

			var identityUser = new IdentityUser();
			await _signInManager.SignInWithClaimsAsync(identityUser, false, result.Value?.Claims.ToArray());

			return Ok(result.Value);
		}
	}
}
