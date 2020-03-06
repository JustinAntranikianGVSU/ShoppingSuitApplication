using System.Collections.Generic;

namespace Domain.Dtos
{
	public class AccessListFullDto : AccessListBasicDto
	{
		public List<LocationBasicDto> Locations { get; set; }

		public List<UserBasicDto> Users { get; set; }

		public AccessListFullDto(int id, string name, List<LocationBasicDto> locations, List<UserBasicDto> users) : base(id, name)
		{
			(Locations, Users) = (locations, users);
		}
	}
}
