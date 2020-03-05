using Domain.Clients;
using System.Collections.Generic;

namespace Domain.Dtos
{
	public class LocationWithAccessListDto
	{
		public int LocationId { get; set; }

		public string LocationName { get; set; }

		public List<AccessListDto> AccessListDtos { get; set; }

		public Client Client { get; set; }

		public LocationWithAccessListDto(int locationId, string locationName, List<AccessListDto> accessListDtos, Client client)
		{
			LocationId = locationId;
			LocationName = locationName;
			AccessListDtos = accessListDtos;
			Client = client;
		}
	}
}
