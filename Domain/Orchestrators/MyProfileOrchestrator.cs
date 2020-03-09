using DataAccess;
using CoreLibrary;
using Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CoreLibrary.ServiceResults;
using CoreLibrary.Orchestrators;
using DataAccess.Repositories;
using System;

namespace Domain.Orchestrators
{
	public interface IMyProfileOrchestrator
	{
		Task<ServiceResult<ProfileWithImpersonationDto>> Get();
	}

	public class MyProfileOrchestrator : JwtContextOrchestratorBase<ProfileWithImpersonationDto>, IMyProfileOrchestrator
	{
		private readonly UsersRepository _usersRepository;
		private readonly UsersWithRolesRepository _usersWithRolesRepository;

		public MyProfileOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext) : base(dbContext, jwtRequestContext)
		{
			_usersRepository = new UsersRepository(_dbContext);
			_usersWithRolesRepository = new UsersWithRolesRepository(_dbContext);
		}

		public async Task<ServiceResult<ProfileWithImpersonationDto>> Get()
		{
			var loggedInUserId = _jwtRequestContext.LoggedInUserId;
			var impersonationUserId = _jwtRequestContext.ImpersonationUserId;

			var loggedInUser = await GetUserProfile(loggedInUserId);
			var impersonationUser = impersonationUserId.HasValue ? await GetUserProfile(impersonationUserId.Value) : null;

			var profileWithImpersonationDto = new ProfileWithImpersonationDto(loggedInUser, impersonationUser);
			return GetProcessedResult(profileWithImpersonationDto);
		}

		private async Task<UserProfileDto> GetUserProfile(int userId)
		{
			var userEntity = await _usersWithRolesRepository.SingleAsync(userId);
			var locations = await GetLocations(userId);
			var clientName = ClientLookup.GetClientName(userEntity.ClientIdentifier);

			return new UserProfileDto(userEntity.Id, userEntity.FirstName, userEntity.LastName, locations, clientName);
		}

		private async Task<List<LocationBasicDto>> GetLocations(int userId)
		{
			var query = _usersRepository.GetLocations(userId);
			var locationDtos = await query.Select(oo => new LocationBasicDto(oo.Id, oo.Name)).ToListAsync();
			return locationDtos;
		}
	}
}
