using AutoMapper;
using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
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
		private readonly IMapper _mapper;

		public GetUserOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext, IMapper mapper) : base(dbContext, jwtRequestContext) => _mapper = mapper;

		public async Task<ServiceResult<List<UserDto>>> GetAll()
		{
			var clientId = _jwtRequestContext.GetClientId();
			var queryable = _dbContext.Users.Include(oo => oo.Roles).AsNoTracking();

			if (clientId.HasValue)
			{
				queryable = queryable.Where(oo => oo.ClientIdentifier == clientId.Value);
			}

			var userEntites = await queryable.ToListAsync();
			var users = new UserMapper(_mapper).Map(userEntites);
			return new ServiceResult<List<UserDto>>(users, ServiceResultStatus.Processed);
		}

		public async Task<ServiceResult<UserDto>> Get(int id)
		{
			var userEntity = await new UsersRepository(_dbContext).SingleOrDefaultAsync(id);

			if (userEntity == null)
			{
				return GetNotFoundResult(GetResourceNotFoundMessage(id));
			}

			var userDto = new UserMapper(_mapper).Map(userEntity);
			return GetProcessedResult(userDto);
		}
	}
}
