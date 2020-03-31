using CoreLibrary;
using DataAccess.Entities;
using Domain.Dtos;
using System.Collections.Generic;

namespace Domain.Orchestrators.AccessLists
{
	public class UpdateAccessListManager
	{
		public class UpdateResult
		{
			public string Name;
			public List<AccessListLocationEntity> Locations;
			public List<UserAccessListEntity> Users;

			public UpdateResult(string name, List<AccessListLocationEntity> accessLists, List<UserAccessListEntity> users)
			{
				Name = name;
				Locations = accessLists;
				Users = users;
			}
		}

		public UpdateResult GetResult(AccessListEntity accessListEntity, AccessListUpdateDto accessListUpdateDto)
		{
			var users = GetUsers(accessListEntity.Users, accessListUpdateDto.UserIds);
			var locations = GetLocations(accessListEntity.Locations, accessListUpdateDto.LocationIds);
			return new UpdateResult(accessListEntity.Name, locations, users);
		}

		private List<AccessListLocationEntity> GetLocations(ICollection<AccessListLocationEntity> locations, List<int> ids)
		{
			var helper = new AddEntitiesRemovingMissingHelper<AccessListLocationEntity, int>
			(
				oo => ids.Contains(oo.LocationId),
				oo => oo.LocationId,
				oo => new AccessListLocationEntity { LocationId = oo }
			);

			return helper.GetEntites(locations, ids);
		}

		private List<UserAccessListEntity> GetUsers(ICollection<UserAccessListEntity> users, List<int> ids)
		{
			var helper = new AddEntitiesRemovingMissingHelper<UserAccessListEntity, int>
			(
				oo => ids.Contains(oo.UserId),
				oo => oo.UserId,
				oo => new UserAccessListEntity { UserId = oo }
			);

			return helper.GetEntites(users, ids);
		}
	}
}
