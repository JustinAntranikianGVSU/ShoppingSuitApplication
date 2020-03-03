using Domain;
using Domain.Constants;
using Domain.Orchestrators;
using Domain.Security;
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

			var claims = HttpContext.User.Claims.ToList();

			HandleUserClaims(claims, impersonatingUserId);
			HandleClientClaims(claims, userResult.Value);

			var jwtToken = new JwtTokenHelper(_config).GenerateJSONWebToken(claims);
			return Ok(new { token = jwtToken });
		}

		private void HandleUserClaims(List<Claim> claims, int impersonatingUserId)
		{
			var claim = HttpContext.User.FindFirst(AppClaimTypes.ImpersonationUserId);

			if (claim is {})
			{
				claims.Remove(claim);
			}

			claims.Add(new Claim(AppClaimTypes.ImpersonationUserId, impersonatingUserId.ToString()));
		}

		private void HandleClientClaims(List<Claim> claims, User userDto)
		{
			var claim = HttpContext.User.FindFirst(AppClaimTypes.ImpersonationClientId);

			if (claim is {})
			{
				claims.Remove(claim);
			}

			var clientId = userDto.ClientIdentifier.HasValue ? userDto.ClientIdentifier : Guid.Empty;
			claims.Add(new Claim(AppClaimTypes.ImpersonationClientId, clientId.ToString()));
		}
	}
}