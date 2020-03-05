using CoreLibrary.RequestContexts;
using Domain;
using Domain.Orchestrators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingSuitePlatform.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingSuitePlatform.Controllers
{
	[Route("[controller]")]
    [ApiController]
    public class ExitImpersonationController : ControllerBase
    {
		private readonly IConfiguration _config;
		private readonly IGetUserOrchestrator _orchestrator;
		private readonly JwtRequestContext _jwtRequestContext;

		public ExitImpersonationController(IConfiguration config, IGetUserOrchestrator orchestrator, JwtRequestContext jwtRequestContext)
		{
			(_config, _orchestrator, _jwtRequestContext) = (config, orchestrator, jwtRequestContext);
		}

		[Authorize()]
		public async Task<ActionResult> Post()
		{
			var userId = _jwtRequestContext.LoggedInUserId;
			var userResult = await _orchestrator.Get(userId);

			if (userResult.Value is null)
			{
				return NotFound(userResult.Errors);
			}

			var claims = HttpContext.User.GetUserAndClientClaims();

			claims.AddRange(GetRoleClaims(userResult.Value));

			var jwtToken = new JwtTokenHelper(_config).GenerateJSONWebToken(claims);
			return Ok(new { token = jwtToken });
		}

		private List<Claim> GetRoleClaims(UserDto user)
		{
			return user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Identifier.ToString())).ToList();
		}
	}
}