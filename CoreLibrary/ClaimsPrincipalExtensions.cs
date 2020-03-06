using System;
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

		/// <summary>
		/// Gets the int for the UserId of the principal.
		/// </summary>
		/// <param name="claimsPrincipal"></param>
		/// <returns></returns>
		public static int GetLoggedInUserId(this ClaimsPrincipal claimsPrincipal)
		{
			var claim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
			return Convert.ToInt32(claim.Value);
		}

		/// <summary>
		/// ClientId claim is an empty guid, but we want to return a nullable Guid.
		/// </summary>
		/// <param name="claimsPrincipal"></param>
		/// <returns></returns>
		public static Guid? GetClientId(this ClaimsPrincipal claimsPrincipal)
		{
			var claim = claimsPrincipal.FindFirst(AppClaimTypes.ClientId);
			var clientId = Guid.Parse(claim.Value);
			return clientId == default ? default(Guid?) : clientId;
		}

		public static int? GetImpersonationUserId(this ClaimsPrincipal claimsPrincipal)
		{
			var claim = claimsPrincipal.FindFirst(AppClaimTypes.ImpersonationUserId);
			return (claim is { Value: var value }) ? Convert.ToInt32(value) : default(int?);
		}

		public static Guid? GetImpersonationClientId(this ClaimsPrincipal claimsPrincipal)
		{
			var claim = claimsPrincipal.FindFirst(AppClaimTypes.ImpersonationClientId);
			return (claim is { Value: var value }) ? Guid.Parse(value) : default(Guid?);
		}
	}
}
