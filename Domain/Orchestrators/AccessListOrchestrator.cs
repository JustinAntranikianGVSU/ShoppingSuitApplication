using DataAccess;
using Domain.Dtos;
using Domain.ServiceResult;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface IAccessListOrchestrator
	{
		Task<ServiceResult<List<AccessListBasicDto>>> Get();

		Task<ServiceResult<AccessListFullDto>> Get(int accessListId);
	}

	public class AccessListOrchestrator : OrchestratorBase<List<AccessListBasicDto>>, IAccessListOrchestrator
	{
		private readonly AppDbContext _dbContext;
		private readonly JwtRequestContext _jwtRequestContext;

		public AccessListOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext)
		{
			(_dbContext, _jwtRequestContext) = (dbContext, jwtRequestContext);
		}

		public async Task<ServiceResult<List<AccessListBasicDto>>> Get()
		{
			var initalQuery = _dbContext.AccessLists.AsNoTracking();
			var clientId = _jwtRequestContext.GetClientId();

			if (clientId.HasValue)
			{
				initalQuery = initalQuery.Where(oo => oo.ClientIdentifier == clientId);
			}

			var accessListEntites = await initalQuery.ToListAsync();
			var accessListDtos = accessListEntites.Select(oo => new AccessListBasicDto(oo.Id, oo.Name));
			return GetProcessedResult(accessListDtos.ToList());
		}

		/// <summary>
		/// For this method we want to return information about the users and locations associated to this access list.
		/// </summary>
		/// <param name="accessListId"></param>
		/// <returns></returns>
		public async Task<ServiceResult<AccessListFullDto>> Get(int accessListId)
		{
			var accessList = await _dbContext.AccessLists
										.Include(oo => oo.UserAccessListEntities).ThenInclude(oo => oo.User)
										.Include(oo => oo.AccessListLocationEntities).ThenInclude(oo => oo.Location)
										.AsNoTracking()
										.SingleAsync(oo => oo.Id == accessListId);

			var users = accessList.UserAccessListEntities.Select(oo => new User(oo.UserId, oo.User.FirstName, oo.User.LastName));
			var locations = accessList.AccessListLocationEntities.Select(oo => new LocationBasicDto(oo.LocationId, oo.Location.Name));

			var accessListDto = new AccessListFullDto(accessList.Id, accessList.Name, locations.ToList(), users.ToList());
			return new ServiceResult<AccessListFullDto>(accessListDto, ServiceResultStatus.Processed);
		}
	}
}
