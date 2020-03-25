using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Entities;
using Domain.Dtos;
using Domain.Entities;
using Domain.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators.Users
{
	public interface IUpdateUserOrchestrator
	{
		public Task<ServiceResult<UserWithLocationsDto>> Update(int id, UserUpdateDto userUpdateDto);
	}

	public class UpdateUserOrchestrator : OrchestratorBase, IUpdateUserOrchestrator
	{
		private readonly UserWithLocationsMapper _userWithLocationsMapper;

		public UpdateUserOrchestrator(AppDbContext dbContext) : base(dbContext) => _userWithLocationsMapper = new UserWithLocationsMapper();

		public async Task<ServiceResult<UserWithLocationsDto>> Update(int id, UserUpdateDto userUpdateDto)
		{
			var userEntity = await GetUserWithRolesAndAccessListsQuery().SingleAsync(oo => oo.Id == id);

			userEntity.FirstName = userUpdateDto.FirstName;
			userEntity.LastName = userUpdateDto.LastName;
			userEntity.Email = userUpdateDto.Email;
			userEntity.Roles = GetRoles(userEntity, userUpdateDto.RoleIds);
			userEntity.AccessLists = GetAccessLists(userEntity, userUpdateDto.AccessListIds);

			await _dbContext.SaveChangesAsync();

			var result = await GetUserWithLocationsDto(id);
			return GetProcessedResult(result);
		}

		private IQueryable<UserEntity> GetUserWithRolesAndAccessListsQuery()
		{
			return _dbContext.Users
						.Include(oo => oo.AccessLists)
						.Include(oo => oo.Roles);
		}

		private IQueryable<UserEntity> GetUserWithLocationsQuery()
		{
			return _dbContext.Users
						.Include(oo => oo.AccessLists)
						.ThenInclude(oo => oo.AccessList)
						.ThenInclude(oo => oo.Locations)
						.ThenInclude(oo => oo.Location)
						.Include(oo => oo.Roles);
		}

		private async Task<UserWithLocationsDto> GetUserWithLocationsDto(int id)
		{
			var userEntity = await GetUserWithLocationsQuery().SingleAsync(oo => oo.Id == id);
			return _userWithLocationsMapper.Map(userEntity);
		}

		private List<UserAccessListEntity> GetAccessLists(UserEntity userEntity, List<int> newAccessListIds)
		{
			var entitesToKeep = userEntity.AccessLists.Where(oo => newAccessListIds.Contains(oo.AccessListId));
			var existingIds = userEntity.AccessLists.Select(oo => oo.AccessListId);
			var entitesToAdd = newAccessListIds.Except(existingIds).Select(oo => new UserAccessListEntity { AccessListId = oo });
			return entitesToKeep.Concat(entitesToAdd).ToList();
		}

		private List<UserRoleEntity> GetRoles(UserEntity userEntity, List<Guid> newRoleIds)
		{
			var entitesToKeep = userEntity.Roles.Where(oo => newRoleIds.Contains(oo.RoleGuid));
			var existingIds = userEntity.Roles.Select(oo => oo.RoleGuid);
			var entitesToAdd = newRoleIds.Except(existingIds).Select(oo => new UserRoleEntity { RoleGuid = oo });
			return entitesToKeep.Concat(entitesToAdd).ToList();
		}
	}
}
