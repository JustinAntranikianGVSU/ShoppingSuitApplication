using AutoMapper;
using DataAccess.Entities;
using Domain.Clients;
using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Mappers
{
	public class LocationWithAccessListMapper
	{
		public LocationWithAccessListDto Map(LocationEntity entity)
		{
			var accessListDtos = entity.AccessListLocationEntities.Select(oo => new AccessListBasicDto(oo.AccessList.Id, oo.AccessList.Name));
			var client = ClientLookup.GetClient(entity.ClientIdentifier);
			return new LocationWithAccessListDto(entity.Id, entity.Name, accessListDtos.ToList(), client);
		}

		public List<LocationWithAccessListDto> Map(List<LocationEntity> entities) => entities.Select(Map).ToList();
	}
}
