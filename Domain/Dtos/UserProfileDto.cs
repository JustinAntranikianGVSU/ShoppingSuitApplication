using System.Collections.Generic;

namespace Domain.Dtos
{
	public class UserProfileDto : UserBasicDto
	{
		public List<LocationBasicDto> Locations { get; set; }

		public string? ClientName { get; set; }

		public UserProfileDto(int id, string firstName, string lastName, List<LocationBasicDto> locations, string? clientName) : base(id, firstName, lastName)
		{
			(Locations, ClientName) = (locations, clientName);
		}
	}
}
