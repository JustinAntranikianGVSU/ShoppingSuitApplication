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

namespace Domain.Orchestrators
{
	public interface IMyProfileOrchestrator
	{
		Task<ServiceResult<MyProfileDto>> Get();
	}

	public class MyProfileOrchestrator : OrchestratorBase<MyProfileDto>, IMyProfileOrchestrator
	{
		private readonly AppDbContext _dbContext;
		private readonly JwtRequestContext _jwtRequestContext;
		private readonly IMapper _mapper;

		public MyProfileOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext, IMapper mapper)
		{
			(_dbContext, _jwtRequestContext, _mapper) = (dbContext, jwtRequestContext, mapper);
		}

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

		private async Task<User?> GetImpersonationUser()
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
			// TODO: this is duplicated
			var query = from userLists in _dbContext.UserAccessLists
						join listLocations in _dbContext.AccessListLocations on userLists.AccessListId equals listLocations.AccessListId
						join locations in _dbContext.Locations on listLocations.LocationId equals locations.Id
						where userLists.UserId == userId
						select new LocationBasicDto(locations.Id, locations.Name);

			var locationDtos = await query.ToListAsync();
			return locationDtos;
		}
	}
}
