using CoreLibrary.Constants;
using CoreLibrary.RequestContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Security.Claims;

namespace ShoppingSuitePlatform.Helpers
{
	/// <summary>
	/// Class to handle proper initalization of the JwtRequestContext. Better than doing it all inline in the OnTokenValidated function.
	/// </summary>
	public class JwtRequestContextHelper
	{
		private readonly TokenValidatedContext _tokenValidatedContext;
		private readonly JwtRequestContext _jwtRequestContext;

		public JwtRequestContextHelper(TokenValidatedContext tokenValidatedContext)
		{
			var jwtUserContext = tokenValidatedContext.HttpContext.RequestServices.GetService(typeof(JwtRequestContext)) as JwtRequestContext;
			_tokenValidatedContext = tokenValidatedContext;
			_jwtRequestContext = jwtUserContext ?? throw new Exception("This should never be null. Make sure the BuildJwtUserContext() is called in OWIN middleware");
		}

		public void InitContext()
		{
			SetUserId();
			SetClientId();
			SetImpersonationId();
			SetImpersonationClientId();
		}

		private void SetUserId()
		{
			_jwtRequestContext.LoggedInUserId = Convert.ToInt32(GetClaimValue(ClaimTypes.NameIdentifier));
		}

		private void SetClientId()
		{
			var clientId = Guid.Parse(GetClaimValue(AppClaimTypes.ClientId));
			if (clientId != default)
			{
				_jwtRequestContext.LoggedInUserClientIdentifier = clientId;
			}
		}

		private void SetImpersonationId()
		{
			var claim = GetClaim(AppClaimTypes.ImpersonationUserId);
			if (claim is { Value: var value })
			{
				_jwtRequestContext.ImpersonationUserId = Convert.ToInt32(value);
			}
		}

		private void SetImpersonationClientId()
		{
			var claim = GetClaim(AppClaimTypes.ImpersonationClientId);
			if (claim is { Value: var value })
			{
				_jwtRequestContext.ImpersonationClientIdentifier = Guid.Parse(value);
			}
		}

		private string GetClaimValue(string claimType) => _tokenValidatedContext.Principal.FindFirstValue(claimType);

		private Claim GetClaim(string claimType) => _tokenValidatedContext.Principal.FindFirst(claimType);
	}
}
