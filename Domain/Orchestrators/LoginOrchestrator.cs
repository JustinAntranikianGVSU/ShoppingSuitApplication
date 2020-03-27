using AutoMapper;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Mappers;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface ILoginOrchestrator
	{
		Task<ServiceResult<List<Claim>>> GetUserClaims(LoginDto loginReqestDto);
	}

	public class LoginOrchestrator : OrchestratorBase<List<Claim>>, ILoginOrchestrator
	{
		private readonly UserMapper _userMapper;
		private readonly UsersWithRolesRepository _usersWithRolesRepository;

		public LoginOrchestrator(AppDbContext dbContext, IMapper mapper) : base(dbContext)
		{
			_userMapper = new UserMapper(mapper);
			_usersWithRolesRepository = new UsersWithRolesRepository(dbContext);
		}

		public async Task<ServiceResult<List<Claim>>> GetUserClaims(LoginDto loginReqestDto)
		{
			var userEntity = await _usersWithRolesRepository.GetByEmail(loginReqestDto.Email);

			if (userEntity is null)
			{
				var message = $"{loginReqestDto.Email} could not be found.";
				var error = GetError(message, nameof(loginReqestDto.Email));
				return GetBadRequestResult(error);
			}

			var userClaims = _userMapper.Map(userEntity).GetClaims();
			return GetProcessedResult(userClaims);
		}
	}
}
