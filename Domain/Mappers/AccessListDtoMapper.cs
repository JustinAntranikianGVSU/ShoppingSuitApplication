using DataAccess.Entities;
using Domain.Dtos;
using System.Linq;

namespace Domain.Mappers
{
	public class AccessListDtoMapper : EntityToDtoMapperBase<AccessListEntity, AccessListDto>
	{
		public override AccessListDto Map(AccessListEntity accessList)
		{
			var users = accessList.Users.Select(oo => new UserBasicDto(oo.UserId, oo.User.FirstName, oo.User.LastName)).ToList();
			var locations = accessList.Locations.Select(oo => new LocationBasicDto(oo.LocationId, oo.Location.Name)).ToList();
			return new AccessListDto(accessList.Id, accessList.Name, locations, users);
		}
	}
}
