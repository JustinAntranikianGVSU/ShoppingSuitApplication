﻿using AutoMapper;
using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.RequestContexts;
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

	public class ImpersonateOrchestrator : JwtContextOrchestratorBase<List<Claim>>, IImpersonateOrchestrator
	{
		private readonly UserMapper _userMapper;
		private readonly UsersRepository _usersRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ImpersonateOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(dbContext, jwtRequestContext)
		{
			_userMapper = new UserMapper(mapper);
			_usersRepository = new UsersRepository(_dbContext);
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
			var userEntity = await _usersRepository.SingleOrDefaultAsync(impersonateUserId);

			if (userEntity is null)
			{
				var error = GetError(GetResourceNotFoundMessage(impersonateUserId));
				return GetNotFoundResult(error);
			}

			var userClaims = _httpContextAccessor.HttpContext.User.GetUserAndClientClaims();
			var impersonateClaims = _userMapper.Map(userEntity).GetUserClaims();

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

			var userEntity = await _usersRepository.SingleAsync(_jwtRequestContext.LoggedInUserId);
			var userClaims = _userMapper.Map(userEntity).GetUserClaims();
			return GetProcessedResult(userClaims);
		}
	}
}
