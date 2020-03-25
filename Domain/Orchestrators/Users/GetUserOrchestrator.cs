using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Orchestrators.Users
{
	public interface IGetUserOrchestrator
	{
		Task<ServiceResult<List<UserWithLocationsDto>>> GetAll();

		Task<ServiceResult<UserWithLocationsDto>> Get(int id);
	}

	public class GetUserOrchestrator : OrchestratorBase, IGetUserOrchestrator
	{
		private readonly UserWithLocationsMapper _userWithLocationsMapper;
		private readonly UsersWithLocationsRepository _usersWithLocationsRepository;

		public GetUserOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext) : base(dbContext, jwtRequestContext)
		{
			_userWithLocationsMapper = new UserWithLocationsMapper();
			_usersWithLocationsRepository = new UsersWithLocationsRepository(dbContext);
		}

		public async Task<ServiceResult<List<UserWithLocationsDto>>> GetAll()
		{
			var clientId = _jwtRequestContext.GetClientId();
			var userEntites = await _usersWithLocationsRepository.GetAll(clientId);
			var userDtos = _userWithLocationsMapper.Map(userEntites);
			return GetProcessedResult(userDtos);
		}

		public async Task<ServiceResult<UserWithLocationsDto>> Get(int id)
		{
			var userEntity = await _usersWithLocationsRepository.SingleAsync(id);
			var userDto = _userWithLocationsMapper.Map(userEntity);
			return GetProcessedResult(userDto);
		}
	}
}
