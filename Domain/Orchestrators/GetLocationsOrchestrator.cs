using DataAccess;
using Domain.Clients;
using Domain.Dtos;
using Domain.ServiceResult;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface IGetLocationsOrchestrator
	{
		Task<ServiceResult<List<LocationWithAccessListDto>>> Get();

		Task<ServiceResult<List<User>>> GetUsersByLocation(int id);
	}

	public class GetLocationsOrchestrator : OrchestratorBase<List<LocationWithAccessListDto>>, IGetLocationsOrchestrator
	{
		private readonly AppDbContext _dbContext;
		private readonly JwtRequestContext _jwtRequestContext;

		public GetLocationsOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext)
		{
			(_dbContext, _jwtRequestContext) = (dbContext, jwtRequestContext);
		}

		public async Task<ServiceResult<List<LocationWithAccessListDto>>> Get()
		{
			var initalQuery = _dbContext.Locations.Include(oo => oo.AccessListLocationEntities).ThenInclude(oo => oo.AccessList).AsNoTracking();
			var clientId = _jwtRequestContext.GetClientId();

			if (clientId.HasValue)
			{
				initalQuery = initalQuery.Where(oo => oo.ClientIdentifier == clientId);
			}

			var locationDtos = await initalQuery.ToListAsync();

			var dtos = locationDtos.Select(oo =>
			{
				var accessListDtos = oo.AccessListLocationEntities.Select(oo => new AccessListDto(oo.AccessList.Id, oo.AccessList.Name));
				var client = ClientLookup.GetClient(oo.ClientIdentifier);
				return new LocationWithAccessListDto(oo.Id, oo.Name, accessListDtos.ToList(), client);
			});

			return GetProcessedResult(dtos.ToList());
		}

		/// <summary>
		/// Gets a list of users assocated to a given location by going through the accessLists.
		/// </summary>
		/// <param name="locationId"></param>
		/// <returns></returns>
		public async Task<ServiceResult<List<User>>> GetUsersByLocation(int locationId)
		{
			var query = from accessListLocations in _dbContext.AccessListLocations
						join accessLists in _dbContext.AccessLists on accessListLocations.AccessListId equals accessLists.Id
						join userAccessList in _dbContext.UserAccessLists on accessLists.Id equals userAccessList.AccessListId
						join users in _dbContext.Users on userAccessList.UserId equals users.Id
						where accessListLocations.LocationId == locationId
						select new User { Id = users.Id, FirstName = users.FirstName, LastName = users.LastName, };

			var locationDtos = await query.ToListAsync();
			return new ServiceResult<List<User>>(locationDtos, ServiceResultStatus.Processed);
		}
	}
}
