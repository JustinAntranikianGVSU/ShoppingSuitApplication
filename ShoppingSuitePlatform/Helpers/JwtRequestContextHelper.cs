using CoreLibrary;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

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
			var principal = _tokenValidatedContext.Principal;
			_jwtRequestContext.LoggedInUserId = principal.GetLoggedInUserId();
			_jwtRequestContext.LoggedInUserClientIdentifier = principal.GetClientId();
			_jwtRequestContext.ImpersonationUserId = principal.GetImpersonationUserId();
			_jwtRequestContext.ImpersonationClientIdentifier = principal.GetImpersonationClientId();
		}
	}
}
