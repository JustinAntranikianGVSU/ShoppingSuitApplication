using System.Collections.Generic;

namespace CoreLibrary
{
	public static class AppPolicy
	{
		public const string DefaultPolicy = "DefaultPolicy";
		public const string ViewEmployee = "ViewEmployee";
		public const string EditEmployee = "EditEmployee";
		public const string DeleteEmployee = "DeleteEmployee";

		public static List<(string policy, Permission permission)> GetPolicyToPermissionMappings()
		{
			return new List<(string, Permission)>()
			{
				(ViewEmployee, Permission.ViewEmployee),
				(EditEmployee, Permission.EditEmployee),
				(DeleteEmployee, Permission.DeleteEmployee),
			};
		}
	}
}
