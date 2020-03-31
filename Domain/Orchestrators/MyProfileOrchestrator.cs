using DataAccess;
using CoreLibrary;
using Domain.Dtos;
using System.Threading.Tasks;
using CoreLibrary.ServiceResults;
using CoreLibrary.Orchestrators;
using DataAccess.Repositories;
using Domain.Mappers;

namespace Domain.Orchestrators
{
	public interface IMyProfileOrchestrator
	{
		Task<ServiceResult<ProfileWithImpersonationDto>> Get();
	}

	public class MyProfileOrchestrator : OrchestratorBase, IMyProfileOrchestrator
	{
		private readonly UsersWithLocationsRepository _usersWithLocationsRepository;

		public MyProfileOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext) : base(dbContext, jwtRequestContext)
		{
			_usersWithLocationsRepository = new UsersWithLocationsRepository(_dbContext);
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

		private async Task<UserWithLocationsDto> GetUserProfile(int userId)
		{
			var userEntity = await _usersWithLocationsRepository.SingleAsync(userId);
			return new UserWithLocationsMapper().Map(userEntity);
		}
	}
}
