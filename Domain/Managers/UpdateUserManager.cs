using CoreLibrary;
using DataAccess.Entities;
using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Domain.Managers
{
	public class UpdateUserManager
	{
		public class UpdateResult
		{
			public string FirstName;
			public string LastName;
			public string Email;
			public List<UserRoleEntity> Roles;
			public List<UserAccessListEntity> AccessLists;

			public UpdateResult(string firstName, string lastName, string email, List<UserRoleEntity> roles, List<UserAccessListEntity> accessLists)
			{
				FirstName = firstName;
				LastName = lastName;
				Email = email;
				Roles = roles;
				AccessLists = accessLists;
			}
		}

		public UpdateResult GetResult(UserEntity userEntity, UserUpdateDto userUpdateDto)
		{
			var roles = GetRoles(userEntity.Roles, userUpdateDto.RoleIds);
			var accessLists = GetAccessLists(userEntity.AccessLists, userUpdateDto.AccessListIds);
			return new UpdateResult(userEntity.FirstName, userEntity.LastName, userEntity.Email, roles, accessLists);
		}

		private List<UserAccessListEntity> GetAccessLists(ICollection<UserAccessListEntity> accessLists, List<int> ids)
		{
			var helper = new AddEntitiesRemovingMissingHelper<UserAccessListEntity, int>
			(
				oo => ids.Contains(oo.AccessListId),
				oo => oo.AccessListId,
				oo => new UserAccessListEntity { AccessListId = oo }
			);

			return helper.GetEntites(accessLists, ids);
		}

		private List<UserRoleEntity> GetRoles(ICollection<UserRoleEntity> roles, List<Guid> ids)
		{
			var helper = new AddEntitiesRemovingMissingHelper<UserRoleEntity, Guid>
			(
				oo => ids.Contains(oo.RoleGuid),
				oo => oo.RoleGuid,
				oo => new UserRoleEntity { RoleGuid = oo }
			);

			return helper.GetEntites(roles, ids);
		}
	}
}
