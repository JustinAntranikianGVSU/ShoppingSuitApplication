using CoreLibrary.Constants;
using Domain;
using Domain.Orchestrators;
using CoreLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingSuitePlatform.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingSuitePlatform.Controllers
{
	[Route("[controller]")]
    [ApiController]
    public class ImpersonationController : ControllerBase
    {
		private readonly IConfiguration _config;

		private readonly IGetUserOrchestrator _orchestrator;

		public ImpersonationController(IConfiguration config, IGetUserOrchestrator orchestrator)
		{
			(_config, _orchestrator) = (config, orchestrator);
		}

		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Post([FromBody] int impersonatingUserId)
		{
			var userResult = await _orchestrator.Get(impersonatingUserId);

			if (userResult.Value is null)
			{
				return NotFound(userResult.Errors);
			}

			var claims = HttpContext.User.GetUserAndClientClaims();

			claims.AddRange(GetRoleClaims(userResult.Value));
			claims.Add(GetImpersonationUserClaim(impersonatingUserId));
			claims.Add(GetImpersonationClientIdClaim(userResult.Value));

			var jwtToken = new JwtTokenHelper(_config).GenerateJSONWebToken(claims);
			return Ok(new { token = jwtToken });
		}

		private List<Claim> GetRoleClaims(User user)
		{
			return user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Identifier.ToString())).ToList();
		}

		private Claim GetImpersonationUserClaim(int impersonatingUserId)
		{
			return new Claim(AppClaimTypes.ImpersonationUserId, impersonatingUserId.ToString());
		}

		private Claim GetImpersonationClientIdClaim(User userDto)
		{
			var clientId = userDto.ClientIdentifier.HasValue ? userDto.ClientIdentifier : Guid.Empty;
			return new Claim(AppClaimTypes.ImpersonationClientId, clientId.ToString());
		}
	}
}