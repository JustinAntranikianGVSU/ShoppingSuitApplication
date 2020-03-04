using Domain.Clients;
using System.Collections.Generic;

namespace Domain.Dtos
{
	public class MyProfileDto
	{
		public User LoggedInUser { get; set; }

		public List<LocationDto> LoggedInUserLocations { get; set; }

		public Client? LoggedInClient { get; set; }

		public bool IsImpersonating { get; set; }

		public User? ImpersonatingUser { get; set; }

		public Client? ImpersonatingClient { get; set; }

		public List<LocationDto>? ImpersonatingUserLocations { get; set; }
	}
}
