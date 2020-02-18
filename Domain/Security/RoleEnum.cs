using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Security
{
	public class Role
	{
		public Guid GuidIdentifier { get; set; }

		public string Name { get; set; }

		[JsonIgnore]
		public List<Permission> Permissions { get; set; }

		public bool HasGlobalPermissions { get; set; }

		public bool HasPermission(Permission permission)
		{
			return Permissions.Contains(permission);
		}

		public Role(Guid id, string name)
		{
			GuidIdentifier = id;
			Name = name;
			Permissions = new List<Permission>();
			HasGlobalPermissions = true;
		}

		public Role(Guid id, string name, params Permission[] permissions)
		{
			GuidIdentifier = id;
			Name = name;
			Permissions = permissions.ToList();
		}
	}
}
