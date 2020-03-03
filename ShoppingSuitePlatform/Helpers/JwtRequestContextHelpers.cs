using Domain.Constants;
using Domain.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Linq;
using System.Security.Claims;

namespace ShoppingSuitePlatform.Helpers
{
	/// <summary>
	/// Class to handle proper initalization of the JwtRequestContext. Better than doing it all inline in the OnTokenValidated function.
	/// </summary>
	public static class JwtRequestContextHelpers
	{
		public static void SetUserId(TokenValidatedContext ctx, JwtRequestContext jwtUserContext)
		{
			var nameClaim = ctx.Principal.Claims.Single(oo => oo.Type == ClaimTypes.NameIdentifier);
			jwtUserContext.LoggedInUserId = Convert.ToInt32(nameClaim.Value);
		}

		public static void SetClientId(TokenValidatedContext ctx, JwtRequestContext jwtUserContext)
		{
			var clientId = Guid.Parse(ctx.Principal.FindFirst(AppClaimTypes.ClientId).Value); // clientId is set upon login.
			if (clientId == default)
			{
				return;
			}

			jwtUserContext.ClientIdentifier = clientId;
		}

		public static void SetImpersonationId(TokenValidatedContext ctx, JwtRequestContext jwtUserContext)
		{
			var claim = ctx.Principal.FindFirst(AppClaimTypes.ImpersonationUserId);
			if (claim is null)
			{
				return;
			}

			jwtUserContext.ImpersonationUserId = Convert.ToInt32(claim.Value);
		}

		public static void SetImpersonationClientId(TokenValidatedContext ctx, JwtRequestContext jwtUserContext)
		{
			var claim = ctx.Principal.FindFirst(AppClaimTypes.ImpersonationClientId);
			if (claim is null)
			{
				return;
			}

			jwtUserContext.ImpersonationClientIdentifier = Guid.Parse(claim.Value);
		}
	}
}
