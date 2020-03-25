using System;
using System.Collections.Generic;

namespace Domain.Dtos
{
	public class UserUpdateDto
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public List<int> AccessListIds { get; set; }

		public List<Guid> RoleIds { get; set; }
	}
}
