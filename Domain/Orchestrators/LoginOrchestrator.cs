using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
using Domain.Dtos;
using System.Collections.Generic;
using System.Linq;
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
		private readonly UsersWithRolesRepository _usersWithRolesRepository;

		public LoginOrchestrator(AppDbContext dbContext) : base(dbContext)
		{
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

			var claimsManager = new ClaimsManager(userEntity.Id, userEntity.ClientIdentifier, userEntity.Roles.Select(oo => oo.RoleGuid));
			var userClaims = claimsManager.GetClaims();
			return GetProcessedResult(userClaims);
		}
	}
}
