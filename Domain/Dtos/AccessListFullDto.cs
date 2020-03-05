using System.Collections.Generic;

namespace Domain.Dtos
{
	public class AccessListFullDto : AccessListBasicDto
	{
		public List<LocationBasicDto> Locations { get; set; }

		public List<User> Users { get; set; }

		public AccessListFullDto(int accessListId, string accessListName, List<LocationBasicDto> locations, List<User> users) : base(accessListId, accessListName)
		{
			(Locations, Users) = (locations, users);
		}
	}
}
