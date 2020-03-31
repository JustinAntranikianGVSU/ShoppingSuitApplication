using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Entities;
using Domain.Managers;
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
		private readonly UsersWithLocationsRepository _usersWithLocationsRepository;

		public UpdateUserOrchestrator(AppDbContext dbContext) : base(dbContext)
		{
			_usersWithLocationsRepository = new UsersWithLocationsRepository(dbContext);
		}

		public async Task<ServiceResult<UserWithLocationsDto>> Update(int id, UserUpdateDto userUpdateDto)
		{
			var userEntity = await GetUserWithRolesAndAccessListsQuery().SingleAsync(oo => oo.Id == id);
			var updateResult = new UpdateUserManager().GetResult(userEntity, userUpdateDto);

			userEntity.FirstName = updateResult.FirstName;
			userEntity.LastName = updateResult.LastName;
			userEntity.Email = updateResult.Email;
			userEntity.Roles = updateResult.Roles;
			userEntity.AccessLists = updateResult.AccessLists;

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
			return new UserWithLocationsMapper().Map(userEntity);
		}
	}
}
