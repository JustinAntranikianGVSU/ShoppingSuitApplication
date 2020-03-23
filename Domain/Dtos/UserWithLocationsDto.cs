using System.Collections.Generic;

namespace Domain.Dtos
{
	public class UserWithLocationsDto : UserDto
	{
		public List<LocationBasicDto> Locations { get; set; } = new List<LocationBasicDto>();

		public List<AccessListBasicDto> AccessLists { get; set; } = new List<AccessListBasicDto>();

		public string? ClientName { get; set; }

		public UserWithLocationsDto(int id, string firstName, string lastName) : base(id, firstName, lastName) {}
	}
}
