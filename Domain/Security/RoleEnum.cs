using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Security
{
	public class Role
	{
		public Guid Identifier { get; set; }

		public string Name { get; set; }

		[JsonIgnore]
		public List<Permission> Permissions { get; set; }

		public bool HasGlobalPermissions { get; set; }

		public bool HasPermission(Permission permission)
		{
			return HasGlobalPermissions || Permissions.Contains(permission);
		}

		public Role(Guid id, string name)
		{
			Identifier = id;
			Name = name;
			Permissions = new List<Permission>();
			HasGlobalPermissions = true;
		}

		public Role(Guid id, string name, params Permission[] permissions)
		{
			Identifier = id;
			Name = name;
			Permissions = permissions.ToList();
		}
	}
}
