using System;
using System.Collections.Generic;

namespace Domain.Dtos
{
	public class UserSearchRequestViewModel
	{
		public enum SortField
		{
			FirstName,
			LastName,
			Email
		}

		public string? FirstName { get; set; }

		public string? LastName { get; set; }

		public string? Email { get; set; }

		public string? AccessListName { get; set; }

		public int? AccessListCount { get; set; }

		public string? LocationName { get; set; }

		public int? LocationCount { get; set; }

		public string? RoleName { get; set; }

		public int? RoleCount { get; set; }

		public List<Guid>? RoleIdsMatchAny { get; set; }

		public List<Guid>? RoleIdsMatchAll { get; set; }

		public int? Skip { get; set; }

		public int? Take { get; set; }

		public SortField? SortByField { get; set; }

		public bool SortDescending { get; set; } = false;
	}
}
