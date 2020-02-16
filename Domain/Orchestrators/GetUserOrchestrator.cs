using AutoMapper;
using DataAccess;
using Domain.Security;
using Domain.ServiceResult;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface IGetUserOrchestrator
	{
		Task<ServiceResult<User>> Get(int id);
	}

	public class GetUserOrchestrator : OrchestratorBase<User>, IGetUserOrchestrator
	{
		private readonly AppDbContext _dbContext;

		private readonly IMapper _mapper;

		public GetUserOrchestrator(AppDbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
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

			var userDto = _mapper.Map<User>(userEntity);
			userDto.Roles = userEntity.Roles.Select(oo => RoleLookup.GetRole(oo.RoleGuid)).ToList();

			return GetProcessedResult(userDto);
		}
	}
}
