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
}
