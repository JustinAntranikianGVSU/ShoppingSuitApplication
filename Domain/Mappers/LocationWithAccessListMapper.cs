using DataAccess.Entities;
using CoreLibrary;
using Domain.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Mappers
{
	public class LocationWithAccessListMapper : EntityToDtoMapperBase<LocationEntity, LocationWithUsersDto>
	{
		public override LocationWithUsersDto Map(LocationEntity entity)
		{
			var client = ClientLookup.GetClient(entity.ClientIdentifier);
			var userAccessLists = entity.AccessLists.SelectMany(oo => oo.AccessList.Users);

			var locationWithUsersDto = new LocationWithUsersDto(entity.Id, entity.Name)
			{
				AccessListDtos = entity.AccessLists.Select(oo => new AccessListBasicDto(oo.AccessList.Id, oo.AccessList.Name)).ToList(),
				Client = client,
				Users = userAccessLists.Select(oo => new UserBasicDto(oo.UserId, oo.User.FirstName, oo.User.LastName)).ToList()
			};

			return locationWithUsersDto;
		}
	}
}
