using System.Collections.Generic;

namespace Domain.Security
{
	public static class AppPolicy
	{
		public static string ViewEmployee = "ViewEmployee";
		public static string EditEmployee = "EditEmployee";
		public static string DeleteEmployee = "DeleteEmployee";

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
