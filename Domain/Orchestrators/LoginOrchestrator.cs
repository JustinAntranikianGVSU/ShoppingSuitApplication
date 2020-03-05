﻿using CoreLibrary.Constants;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface ILoginOrchestrator
	{
		Task<ServiceResult<LoginReponseDto>> GetLoginReponse(LoginRequestDto loginReqestDto);
	}

	public class LoginOrchestrator : OrchestratorBase<LoginReponseDto>, ILoginOrchestrator
	{
		private readonly AppDbContext _dbContext;

		public LoginOrchestrator(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<ServiceResult<LoginReponseDto>> GetLoginReponse(LoginRequestDto loginReqestDto)
		{
			var userEntity = await _dbContext.Users.Include(oo => oo.Roles).SingleOrDefaultAsync(oo => oo.Email == loginReqestDto.Email);

			if (userEntity is null)
			{
				var message = $"{loginReqestDto.Email} could not be found.";
				var error = GetError(message, nameof(loginReqestDto.Email));
				return GetBadRequestResult(error);
			}

			var claims = GetClaims(userEntity).ToList();
			return GetProcessedResult(new LoginReponseDto(claims, userEntity.Id));
		}

		private static IEnumerable<Claim> GetClaims(UserEntity userEntity)
		{
			foreach (var role in userEntity.Roles)
			{
				yield return new Claim(ClaimTypes.Role, role.RoleGuid.ToString());
			}

			yield return new Claim(JwtRegisteredClaimNames.Sub, userEntity.Id.ToString());

			var clientId = userEntity.ClientIdentifier.HasValue ? userEntity.ClientIdentifier : Guid.Empty;
			yield return new Claim(AppClaimTypes.ClientId, clientId.ToString());
		}
	}
}
