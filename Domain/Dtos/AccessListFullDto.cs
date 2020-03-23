using System.Collections.Generic;

namespace Domain.Dtos
{
	public class AccessListDto : AccessListBasicDto
	{
		public List<LocationBasicDto> Locations { get; set; }

		public List<UserBasicDto> Users { get; set; }

		public AccessListDto(int id, string name, List<LocationBasicDto> locations, List<UserBasicDto> users) : base(id, name)
		{
			(Locations, Users) = (locations, users);
		}
	}
}
