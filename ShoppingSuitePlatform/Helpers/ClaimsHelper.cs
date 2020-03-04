using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingSuitePlatform.Helpers
{
	public static class ClaimsHelper
	{
		public static List<Claim> GetUserAndClientClaims(this ClaimsPrincipal claimsPrincipal)
		{
			return new List<Claim>
			{
				claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier),
				claimsPrincipal.FindFirst(AppClaimTypes.ClientId)
			};
		}
	}
}
