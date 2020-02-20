using Domain.Security;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingSuitePlatform.MiddleWare
{
	public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
		{
			if (!context.User.Identity.IsAuthenticated)
			{
				return Task.CompletedTask;
			}

			var roleClaims = context.User.FindAll(ClaimTypes.Role);
			foreach (var roleClaim in roleClaims)
			{
				var roleFromLookup = RoleLookup.GetRole(Guid.Parse(roleClaim.Value));

				if (roleFromLookup.HasPermission(requirement.Permission))
				{
					context.Succeed(requirement);
					return Task.CompletedTask;
				}
			}

			return Task.CompletedTask;
		}
	}
}
