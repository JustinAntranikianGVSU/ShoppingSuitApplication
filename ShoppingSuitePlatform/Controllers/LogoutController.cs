using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LogoutController
	{
		private readonly SignInManager<IdentityUser> _signInManager;

		public LogoutController(SignInManager<IdentityUser> signInManager)
		{
			_signInManager = signInManager;
		}

		[HttpPost]
		public async Task<ActionResult> Post()
		{
			await _signInManager.SignOutAsync();
			return new OkResult();
		}
	}
}
