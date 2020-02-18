using Domain.Security;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingSuitePlatform.MiddleWare
{
	public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
		{
			if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
			{
				return Task.CompletedTask;
			}

			var roleIdentifiers = context.User.FindAll(ClaimTypes.Role).Select(oo => Guid.Parse(oo.Value)).ToList();

			foreach (var roleFromLookup in roleIdentifiers.Select(oo => RoleLookup.GetRole(oo)))
			{
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
