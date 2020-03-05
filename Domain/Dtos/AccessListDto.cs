using System.Collections.Generic;

namespace Domain.Dtos
{
	public class AccessListDto
	{
		public int AccessListId { get; set; }

		public string AccessListName { get; set; }

		public AccessListDto(int accessListId, string accessListName)
		{
			AccessListId = accessListId;
			AccessListName = accessListName;
		}
	}

	public class AccessListFullDto : AccessListDto
	{
		public List<LocationDto> Locations { get; set; }

		public List<User> Users { get; set; }

		public AccessListFullDto(int accessListId, string accessListName) : base(accessListId, accessListName) { }
	}
}
