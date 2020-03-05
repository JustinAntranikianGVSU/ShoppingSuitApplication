using CoreLibrary;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingSuitePlatform.MiddleWare
{
	public class PermissionRequirement : IAuthorizationRequirement
	{
		public Permission Permission { get; set; }

		public PermissionRequirement(Permission permission)
		{
			Permission = permission;
		}
	}
}
