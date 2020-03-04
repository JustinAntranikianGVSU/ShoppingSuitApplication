using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Security
{
	public static class RoleLookup
	{
		public static Guid TrainingUserRoleGuid = new Guid("9158b464-23cb-4bd9-b4a1-e1ea0a504177");

		public static Guid TrainingAdminRoleGuid = new Guid("3868c900-41d0-40d0-865c-d6e182568469");

		public static Guid SystemAdminRoleGuid = new Guid("31f8f949-6dc5-43a4-8aa3-f6d3504ed6b7");

		public static List<Role> Roles;

		static RoleLookup()
		{
			var traningUser = new Role(TrainingUserRoleGuid, "Training User", Permission.ViewEmployee);
			var trainingAdmin = new Role(TrainingAdminRoleGuid, "Training Admin", Permission.ViewEmployee, Permission.EditEmployee);
			var systemAdmin = new Role(SystemAdminRoleGuid, "System Admin");

			Roles = new List<Role> { traningUser, trainingAdmin, systemAdmin };
		}

		public static Role GetRole(Guid identifier)
		{
			var role = Roles.SingleOrDefault(oo => oo.Identifier == identifier);

			if (role is null)
			{
				throw new Exception($"Could not find Role with identifier {identifier}. Please fix whatever data is refering to a role that doesn't exist.");
			}

			return role;
		}
	}
}
