using CoreLibrary;
using System.Collections.Generic;

namespace Domain.Dtos
{
	public class LocationWithAccessListDto : LocationBasicDto
	{
		public List<AccessListBasicDto> AccessListDtos { get; set; }

		public Client Client { get; set; }

		public LocationWithAccessListDto(int id, string name, List<AccessListBasicDto> accessListDtos, Client client) : base(id, name)
		{
			AccessListDtos = accessListDtos;
			Client = client;
		}
	}
}
