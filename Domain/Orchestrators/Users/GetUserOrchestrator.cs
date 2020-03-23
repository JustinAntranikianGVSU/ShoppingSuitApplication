using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Entities;
using Domain.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators.Users
{
	public interface IGetUserOrchestrator
	{
		Task<ServiceResult<List<UserWithLocationsDto>>> GetAll();

		Task<ServiceResult<UserWithLocationsDto>> Get(int id);
	}

	public class GetUserOrchestrator : JwtContextOrchestratorBase<UserWithLocationsDto>, IGetUserOrchestrator
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
			var userEntites = await GetUsersQuery().ToListAsync();
			var users = _userWithLocationsMapper.Map(userEntites);
			return GetProcessedResult(users);
		}

		private IQueryable<UserEntity> GetUsersQuery()
		{
			var clientId = _jwtRequestContext.GetClientId();
			var queryable = _usersWithLocationsRepository.GetReadOnlyQuery();
			return clientId.HasValue ? queryable.Where(oo => oo.ClientIdentifier == clientId.Value) : queryable;
		}

		public async Task<ServiceResult<UserWithLocationsDto>> Get(int id)
		{
			var userEntity = await _usersWithLocationsRepository.GetReadOnlyQuery().SingleOrDefaultAsync(oo => oo.Id == id);

			if (userEntity is null)
			{
				var message = GetResourceNotFoundMessage(id);
				return GetNotFoundResult(message);
			}

			var userDto = _userWithLocationsMapper.Map(userEntity);
			return GetProcessedResult(userDto);
		}
	}
}
