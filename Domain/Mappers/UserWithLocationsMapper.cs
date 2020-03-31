using CoreLibrary;
using Domain.Dtos;
using Domain.Entities;
using System.Linq;

namespace Domain.Mappers
{
	public class UserWithLocationsMapper : EntityToDtoMapperBase<UserEntity, UserWithLocationsDto>
	{
		public override UserWithLocationsDto Map(UserEntity userEntity)
		{
			var locationEntites = userEntity.AccessLists.SelectMany(oo => oo.AccessList.Locations);

			var userDto = new UserWithLocationsDto()
			{
				Id = userEntity.Id,
				FirstName = userEntity.FirstName,
				LastName = userEntity.LastName,
				Email = userEntity.Email,
				Roles = userEntity.Roles.Select(oo => RoleLookup.GetRole(oo.RoleGuid)).ToList(),
				ClientIdentifier = userEntity.ClientIdentifier,
				ClientName = ClientLookup.GetClientName(userEntity.ClientIdentifier),
				AccessLists = userEntity.AccessLists.Select(oo => new AccessListBasicDto(oo.AccessList.Id, oo.AccessList.Name)).ToList(),
				Locations = locationEntites.Select(oo => new LocationBasicDto(oo.Location.Id, oo.Location.Name)).ToList()
			};

			return userDto;
		}
	}
}
