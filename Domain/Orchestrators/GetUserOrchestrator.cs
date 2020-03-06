using AutoMapper;
using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
using Domain.Entities;
using Domain.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface IGetUserOrchestrator
	{
		Task<ServiceResult<List<UserDto>>> GetAll();

		Task<ServiceResult<UserDto>> Get(int id);
	}

	public class GetUserOrchestrator : JwtContextOrchestratorBase<UserDto>, IGetUserOrchestrator
	{
		private readonly UserMapper _userMapper;
		private readonly UsersWithRolesRepository _usersWithRolesRepository;

		public GetUserOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext, IMapper mapper) : base(dbContext, jwtRequestContext)
		{
			_userMapper = new UserMapper(mapper);
			_usersWithRolesRepository = new UsersWithRolesRepository(_dbContext);
		}

		public async Task<ServiceResult<List<UserDto>>> GetAll()
		{
			var userEntites = await GetUsersQuery().ToListAsync();
			var users = _userMapper.Map(userEntites);
			return GetProcessedResult(users);
		}

		private IQueryable<UserEntity> GetUsersQuery()
		{
			var clientId = _jwtRequestContext.GetClientId();
			var queryable = _usersWithRolesRepository.GetReadOnlyQuery();
			return clientId.HasValue ? queryable.Where(oo => oo.ClientIdentifier == clientId.Value) : queryable;
		}

		public async Task<ServiceResult<UserDto>> Get(int id)
		{
			var userEntity = await _usersWithRolesRepository.SingleOrDefaultAsync(id);

			if (userEntity is null)
			{
				var message = GetResourceNotFoundMessage(id);
				return GetNotFoundResult(message);
			}

			var userDto = _userMapper.Map(userEntity);
			return GetProcessedResult(userDto);
		}
	}
}
