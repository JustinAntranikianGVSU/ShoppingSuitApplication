using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Security
{
	public class Role
	{
		public Guid Id;

		public string Name;

		public List<Permission> Permissions;

		public bool HasGlobalPermissions;

		public Role(Guid id, string name)
		{
			Id = id;
			Name = name;
			Permissions = new List<Permission>();
			HasGlobalPermissions = true;
		}

		public Role(Guid id, string name, params Permission[] permissions)
		{
			Id = id;
			Name = name;
			Permissions = permissions.ToList();
		}
	}

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
	}
}
