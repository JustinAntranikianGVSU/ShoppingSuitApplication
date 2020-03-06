using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using CoreLibrary.ServiceResults;
using ShoppingSuitePlatform.Helpers;
using Microsoft.Extensions.Configuration;

namespace ShoppingSuitePlatform.Controllers.BaseControllers
{
	public abstract class TokenGeneratorControllerBase : ControllerBase
	{
		private readonly IConfiguration _config;

		protected TokenGeneratorControllerBase(IConfiguration config)
		{
			_config = config;
		}

		protected ActionResult GetTokenResult(ServiceResult<List<Claim>> result)
		{
			if (result.NotProcessed)
			{
				return BadRequest(result.Errors);
			}

			var jwtToken = new JwtTokenHelper(_config).GenerateJSONWebToken(result.Value);
			return Ok(new { token = jwtToken });
		}
	}
}
