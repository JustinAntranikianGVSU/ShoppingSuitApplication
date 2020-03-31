using CoreLibrary;
using Domain.Entities;
using System.Linq;

namespace Domain.Mappers
{
	public class UserMapper : EntityToDtoMapperBase<UserEntity, UserDto>
	{
		public override UserDto Map(UserEntity userEntity)
		{
			return new UserDto()
			{
				Id = userEntity.Id,
				FirstName = userEntity.FirstName,
				LastName = userEntity.LastName,
				Email = userEntity.Email,
				ClientIdentifier = userEntity.ClientIdentifier,
				Roles = userEntity.Roles.Select(oo => RoleLookup.GetRole(oo.RoleGuid)).ToList()
			};
		}
	}
}
