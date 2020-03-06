using AutoMapper;
using DataAccess;
using CoreLibrary;
using Domain.Dtos;
using Domain.Mappers;
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
		Task<ServiceResult<MyProfileDto>> Get();
	}

	public class MyProfileOrchestrator : JwtContextOrchestratorBase<MyProfileDto>, IMyProfileOrchestrator
	{
		private readonly UserMapper _userMapper;
		private readonly UsersRepository _usersRepository;
		private readonly UsersWithRolesRepository _usersWithRolesRepository;

		public MyProfileOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext, IMapper mapper) : base(dbContext, jwtRequestContext)
		{
			_userMapper = new UserMapper(mapper);
			_usersRepository = new UsersRepository(_dbContext);
			_usersWithRolesRepository = new UsersWithRolesRepository(_dbContext);
		}

		public async Task<ServiceResult<MyProfileDto>> Get()
		{
			var context = _jwtRequestContext;
			var userId = context.LoggedInUserId;
			var clientId = context.LoggedInUserClientIdentifier;

			var impersonateUserId = context.ImpersonationUserId;
			var impersonateClientId = context.ImpersonationClientIdentifier;

			var myProfleDto = new MyProfileDto
			{
				LoggedInUser = await GetUser(userId),
				LoggedInUserLocations = await GetLocations(userId),
				LoggedInClient = GetClient(clientId),
				IsImpersonating = impersonateUserId.HasValue,
				ImpersonatingUser = impersonateUserId.HasValue ? await GetUser(impersonateUserId.Value) : null,
				ImpersonatingClient = GetClient(impersonateClientId),
				ImpersonatingUserLocations = impersonateUserId.HasValue ? await GetLocations(impersonateUserId.Value) : null
			};

			return GetProcessedResult(myProfleDto);
		}

		private async Task<List<LocationBasicDto>> GetLocations(int userId)
		{
			var query = _usersRepository.GetLocations(userId);
			var locationDtos = await query.Select(oo => new LocationBasicDto(oo.Id, oo.Name)).ToListAsync();
			return locationDtos;
		}

		private async Task<UserDto> GetUser(int userId)
		{
			var userEntity = await _usersWithRolesRepository.SingleAsync(userId);
			return _userMapper.Map(userEntity);
		}

		private Client? GetClient(Guid? clientId) => clientId.HasValue ? ClientLookup.GetClient(clientId.Value) : null;
	}
}
