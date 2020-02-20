using Domain.Dtos;
using Domain.Orchestrators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingSuitePlatform.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LoginController : ControllerBase
	{
		private readonly ILoginOrchestrator _loginOrchestrator;

		private readonly IConfiguration _config;

		public LoginController(ILoginOrchestrator loginOrchestrator, IConfiguration config)
		{
			(_loginOrchestrator, _config) = (loginOrchestrator, config);
		}

		[HttpPost]
		public async Task<ActionResult> Post([FromBody] LoginRequestDto loginRequestDto)
		{
			var result = await _loginOrchestrator.GetLoginReponse(loginRequestDto);

			if (result.Errors.Any())
			{
				return BadRequest(result.Errors);
			}

			var tokenHelper = new JwtTokenHelper(_config);
			var jwtToken = tokenHelper.GenerateJSONWebToken(result.Value?.Claims ?? new List<Claim>());
			return Ok(new { token = jwtToken });
		}
	}
}
