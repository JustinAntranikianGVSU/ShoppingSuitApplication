using Domain.Dtos;
using Domain.Orchestrators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingSuitePlatform.Controllers.BaseControllers;
using System.Threading.Tasks;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LoginController : TokenGeneratorControllerBase
	{
		private readonly ILoginOrchestrator _loginOrchestrator;

		public LoginController(ILoginOrchestrator loginOrchestrator, IConfiguration config) : base(config)
		{
			_loginOrchestrator = loginOrchestrator;
		}

		[HttpPost]
		public async Task<ActionResult> Post([FromBody] LoginDto loginRequestDto)
		{
			var result = await _loginOrchestrator.GetUserClaims(loginRequestDto);
			return GetTokenResult(result);
		}
	}
}
