using AutoMapper;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface ILoginOrchestrator
	{
		Task<ServiceResult<List<Claim>>> GetLoginReponse(LoginRequestDto loginReqestDto);
	}

	public class LoginOrchestrator : DbContextOrchestratorBase<List<Claim>>, ILoginOrchestrator
	{
		private readonly UserMapper _userMapper;
		private readonly UsersRepository _usersRepository;

		public LoginOrchestrator(AppDbContext dbContext, IMapper mapper) : base(dbContext)
		{
			_userMapper = new UserMapper(mapper);
			_usersRepository = new UsersRepository(_dbContext);
		}

		public async Task<ServiceResult<List<Claim>>> GetLoginReponse(LoginRequestDto loginReqestDto)
		{
			var userEntity = await _usersRepository.GetRolesQuery().SingleOrDefaultAsync(oo => oo.Email == loginReqestDto.Email);

			if (userEntity is null)
			{
				var message = $"{loginReqestDto.Email} could not be found.";
				var error = GetError(message, nameof(loginReqestDto.Email));
				return GetBadRequestResult(error);
			}

			var userClaims = _userMapper.Map(userEntity).GetUserClaims();
			return GetProcessedResult(userClaims);
		}
	}
}
