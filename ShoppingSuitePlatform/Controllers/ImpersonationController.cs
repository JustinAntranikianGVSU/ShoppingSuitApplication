using Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingSuitePlatform.Helpers;
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

		public ImpersonationController(IConfiguration config)
		{
			_config = config;
		}

		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Post([FromBody] int impersonatingUserId)
		{
			var claims = HttpContext.User.Claims.ToList();
			claims.Add(new Claim("impersionationUserId", impersonatingUserId.ToString()));

			var tokenHelper = new JwtTokenHelper(_config);
			var jwtToken = tokenHelper.GenerateJSONWebToken(claims);
			return Ok(new { token = jwtToken });
		}
	}
}