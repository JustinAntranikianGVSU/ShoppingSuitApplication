using AutoMapper;
using DataAccess;
using CoreLibrary;
using Domain.Dtos;
using Domain.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CoreLibrary.RequestContexts;
using CoreLibrary.ServiceResults;
using CoreLibrary.Orchestrators;
using DataAccess.Repositories;

namespace Domain.Orchestrators
{
	public interface IMyProfileOrchestrator
	{
		Task<ServiceResult<MyProfileDto>> Get();
	}

	public class MyProfileOrchestrator : JwtContextOrchestratorBase<MyProfileDto>, IMyProfileOrchestrator
	{
		private readonly IMapper _mapper;

		public MyProfileOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext, IMapper mapper) : base(dbContext, jwtRequestContext) => _mapper = mapper;

		public async Task<ServiceResult<MyProfileDto>> Get()
		{
			var loggedInUserId = _jwtRequestContext.LoggedInUserId;
			var userEntity = await _dbContext.Users.Include(oo => oo.Roles).AsNoTracking().SingleOrDefaultAsync(oo => oo.Id == loggedInUserId);
			var userDto = new UserMapper(_mapper).Map(userEntity);

			var loggedInClient = _jwtRequestContext.LoggedInUserClientIdentifier.HasValue ? ClientLookup.GetClient(_jwtRequestContext.LoggedInUserClientIdentifier.Value) : null;

			var myProfleDto = new MyProfileDto
			{
				LoggedInUser = userDto,
				LoggedInClient = loggedInClient,
				IsImpersonating = _jwtRequestContext.ImpersonationUserId.HasValue,
				ImpersonatingUser = await GetImpersonationUser(),
				ImpersonatingClient = GetImpersonationClient(),
				LoggedInUserLocations = await GetLocations(_jwtRequestContext.LoggedInUserId),
				ImpersonatingUserLocations = _jwtRequestContext.ImpersonationUserId.HasValue ? await GetLocations(_jwtRequestContext.ImpersonationUserId.Value) : null
			};

			return GetProcessedResult(myProfleDto);
		}

		private async Task<UserDto?> GetImpersonationUser()
		{
			if (!_jwtRequestContext.ImpersonationUserId.HasValue)
			{
				return null;
			}

			var userEntity = await _dbContext.Users.Include(oo => oo.Roles).AsNoTracking().SingleOrDefaultAsync(oo => oo.Id == _jwtRequestContext.ImpersonationUserId);
			var userDto = new UserMapper(_mapper).Map(userEntity);
			return userDto;
		}

		private Client? GetImpersonationClient()
		{
			if (!_jwtRequestContext.ImpersonationClientIdentifier.HasValue)
			{
				return null;
			}

			return ClientLookup.GetClient(_jwtRequestContext.ImpersonationClientIdentifier.Value);
		}

		private async Task<List<LocationBasicDto>> GetLocations(int userId)
		{
			var query = new UsersRepository(_dbContext).GetLocations(userId);
			var locationDtos = await query.Select(oo => new LocationBasicDto(oo.Id, oo.Name)).ToListAsync();
			return locationDtos;
		}
	}
}
