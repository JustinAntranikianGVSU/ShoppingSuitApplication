using CoreLibrary.Constants;
using System.Security.Claims;

namespace CoreLibrary
{
	public static class ClaimsPrincipalExtensions
	{
		public static Claim[] GetUserAndClientClaims(this ClaimsPrincipal claimsPrincipal)
		{
			return new []
			{
				claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier),
				claimsPrincipal.FindFirst(AppClaimTypes.ClientId)
			};
		}
	}
}
