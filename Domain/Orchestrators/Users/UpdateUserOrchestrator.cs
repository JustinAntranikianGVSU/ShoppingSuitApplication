using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
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
		private readonly UsersWithLocationsRepository _usersWithLocationsRepository;

		public UpdateUserOrchestrator(AppDbContext dbContext) : base(dbContext)
		{
			_userWithLocationsMapper = new UserWithLocationsMapper();
			_usersWithLocationsRepository = new UsersWithLocationsRepository(dbContext);
		}

		public async Task<ServiceResult<UserWithLocationsDto>> Update(int id, UserUpdateDto userUpdateDto)
		{
			var userEntity = await GetUserWithRolesAndAccessListsQuery().SingleAsync(oo => oo.Id == id);

			userEntity.FirstName = userUpdateDto.FirstName;
			userEntity.LastName = userUpdateDto.LastName;
			userEntity.Email = userUpdateDto.Email;
			userEntity.Roles = GetRoles(userEntity.Roles, userUpdateDto.RoleIds);
			userEntity.AccessLists = GetAccessLists(userEntity.AccessLists, userUpdateDto.AccessListIds);

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

		private async Task<UserWithLocationsDto> GetUserWithLocationsDto(int id)
		{
			var userEntity = await _usersWithLocationsRepository.SingleAsync(id);
			return _userWithLocationsMapper.Map(userEntity);
		}

		private List<UserAccessListEntity> GetAccessLists(ICollection<UserAccessListEntity> accessLists, List<int> ids)
		{
			var entitesToKeep = accessLists.Where(oo => ids.Contains(oo.AccessListId));
			var existingIds = accessLists.Select(oo => oo.AccessListId);
			var entitesToAdd = ids.Except(existingIds).Select(oo => new UserAccessListEntity { AccessListId = oo });
			return entitesToKeep.Concat(entitesToAdd).ToList();
		}

		private List<UserRoleEntity> GetRoles(ICollection<UserRoleEntity> roles, List<Guid> ids)
		{
			var entitesToKeep = roles.Where(oo => ids.Contains(oo.RoleGuid));
			var existingIds = roles.Select(oo => oo.RoleGuid);
			var entitesToAdd = ids.Except(existingIds).Select(oo => new UserRoleEntity { RoleGuid = oo });
			return entitesToKeep.Concat(entitesToAdd).ToList();
		}
	}
}
