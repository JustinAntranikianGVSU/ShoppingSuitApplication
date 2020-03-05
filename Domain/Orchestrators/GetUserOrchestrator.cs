﻿using AutoMapper;
using CoreLibrary.Orchestrators;
using CoreLibrary.RequestContexts;
using CoreLibrary.ServiceResults;
using DataAccess;
using Domain.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface IGetUserOrchestrator
	{
		Task<ServiceResult<List<User>>> GetAll();

		Task<ServiceResult<User>> Get(int id);
	}

	public class GetUserOrchestrator : OrchestratorBase<User>, IGetUserOrchestrator
	{
		private readonly AppDbContext _dbContext;
		private readonly JwtRequestContext _jwtRequestContext;
		private readonly IMapper _mapper;

		public GetUserOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext, IMapper mapper)
		{
			(_dbContext, _jwtRequestContext, _mapper) = (dbContext, jwtRequestContext, mapper);
		}

		public async Task<ServiceResult<List<User>>> GetAll()
		{
			var clientId = _jwtRequestContext.GetClientId();
			var queryable = _dbContext.Users.Include(oo => oo.Roles).AsNoTracking();

			if (clientId.HasValue)
			{
				queryable = queryable.Where(oo => oo.ClientIdentifier == clientId.Value);
			}

			var userEntites = await queryable.ToListAsync();
			var users = new UserMapper(_mapper).Map(userEntites);
			return new ServiceResult<List<User>>(users, ServiceResultStatus.Processed);
		}

		public async Task<ServiceResult<User>> Get(int id)
		{
			var userEntity = await _dbContext.Users.Include(oo => oo.Roles).AsNoTracking().SingleOrDefaultAsync(oo => oo.Id == id);

			if (userEntity == null)
			{
				var message = GetResourceNotFoundMessage(id);
				var error = GetError(message);
				return GetNotFoundResult(error);
			}

			var userDto = new UserMapper(_mapper).Map(userEntity);
			return GetProcessedResult(userDto);
		}
	}
}
