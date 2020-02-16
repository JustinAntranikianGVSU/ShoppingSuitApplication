using AutoMapper;
using DataAccess;
using Domain.Mappers;
using Domain.Security;
using Domain.ServiceResult;
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

		private readonly IMapper _mapper;

		public GetUserOrchestrator(AppDbContext dbContext, IMapper mapper)
		{
			(_dbContext, _mapper) = (dbContext, mapper);
		}

		public async Task<ServiceResult<List<User>>> GetAll()
		{
			var userEntites = await _dbContext.Users.Include(oo => oo.Roles).ToListAsync();
			var users = new UserMapper(_mapper).Map(userEntites);
			return new ServiceResult<List<User>>(users, ServiceResultStatus.Processed);
		}

		public async Task<ServiceResult<User>> Get(int id)
		{
			var userEntity = await _dbContext.Users.Include(oo => oo.Roles).SingleOrDefaultAsync(oo => oo.Id == id);

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
