using CoreLibrary;
using System;
using System.Collections.Generic;

namespace Domain
{
	public class UserDto : UserBasicDto
	{
		public string Email { get; set; }

		public List<Role> Roles { get; set; } = new List<Role>();

		public Guid? ClientIdentifier { get; set; }
	}
}
