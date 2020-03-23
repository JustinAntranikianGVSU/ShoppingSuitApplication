using CoreLibrary;
using System.Collections.Generic;

namespace Domain.Dtos
{
	public class LocationWithUsersDto : LocationBasicDto
	{
		public List<AccessListBasicDto> AccessListDtos { get; set; }

		public List<UserBasicDto> Users { get; set; }

		public Client Client { get; set; }

		public LocationWithUsersDto(int id, string name) : base(id, name) { }
	}
}
