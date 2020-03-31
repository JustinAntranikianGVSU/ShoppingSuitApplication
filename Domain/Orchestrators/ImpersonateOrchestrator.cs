using AutoMapper;
using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
using Domain.Mappers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface IImpersonateOrchestrator
	{
		Task<ServiceResult<List<Claim>>> GetImpersonateClaims(int impersonateUserId);

		Task<ServiceResult<List<Claim>>> GetExitImpersonateClaims();
	}

	public class ImpersonateOrchestrator : OrchestratorBase<List<Claim>>, IImpersonateOrchestrator
	{
		private readonly UsersWithRolesRepository _usersWithRolesRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ImpersonateOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(dbContext, jwtRequestContext)
		{
			_usersWithRolesRepository = new UsersWithRolesRepository(_dbContext);
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// Perserves the principal identity of the user, along with the clientId.
		/// Roles will be changed to the user that you are trying to impersonate.
		/// </summary>
		/// <param name="impersonateUserId"></param>
		/// <returns></returns>
		public async Task<ServiceResult<List<Claim>>> GetImpersonateClaims(int impersonateUserId)
		{
			var userEntity = await _usersWithRolesRepository.SingleOrDefaultAsync(impersonateUserId);

			if (userEntity is null)
			{
				var errorMessage = GetResourceNotFoundMessage(impersonateUserId);
				return GetBadRequestResult(errorMessage);
			}

			var userClaims = _httpContextAccessor.HttpContext.User.GetUserAndClientClaims();

			var claimsManager = new ClaimsManager(userEntity.Id, userEntity.ClientIdentifier, userEntity.Roles.Select(oo => oo.RoleGuid));
			var impersonateClaims = claimsManager.GetClaimsForImpersonation();
			var combinedClaims = userClaims.Concat(impersonateClaims).ToList();
			return GetProcessedResult(combinedClaims);
		}

		public async Task<ServiceResult<List<Claim>>> GetExitImpersonateClaims()
		{
			if (!_jwtRequestContext.ImpersonationUserId.HasValue)
			{
				var errorMessage = "You are not impersonating anyone.";
				return GetBadRequestResult(errorMessage);
			}

			var userEntity = await _usersWithRolesRepository.SingleAsync(_jwtRequestContext.LoggedInUserId);

			var claimsManager = new ClaimsManager(userEntity.Id, userEntity.ClientIdentifier, userEntity.Roles.Select(oo => oo.RoleGuid));
			var userClaims = claimsManager.GetClaims();
			return GetProcessedResult(userClaims);
		}
	}
}
