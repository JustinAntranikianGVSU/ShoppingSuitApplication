using CoreLibrary;
using System.Collections.Generic;

namespace Domain.Dtos
{
	public class MyProfileDto
	{
		public UserDto LoggedInUser { get; set; }

		public List<LocationBasicDto> LoggedInUserLocations { get; set; }

		public Client? LoggedInClient { get; set; }

		public bool IsImpersonating { get; set; }

		public UserDto? ImpersonatingUser { get; set; }

		public Client? ImpersonatingClient { get; set; }

		public List<LocationBasicDto>? ImpersonatingUserLocations { get; set; }
	}
}
