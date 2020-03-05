using DataAccess;
using Domain.ServiceResult;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface IGetUsersByLocationOrchestrator
	{
		Task<ServiceResult<List<User>>> Get(int id);
	}

	public class GetUsersByLocationOrchestrator : DbContextOrchestratorBase<List<User>>, IGetUsersByLocationOrchestrator
	{
		public GetUsersByLocationOrchestrator(AppDbContext dbContext) : base(dbContext) {}

		/// <summary>
		/// Gets a list of users assocated to a given location by going through the accessLists.
		/// </summary>
		/// <param name="locationId"></param>
		/// <returns></returns>
		public async Task<ServiceResult<List<User>>> Get(int locationId)
		{
			var query = from accessListLocations in _dbContext.AccessListLocations
						join accessLists in _dbContext.AccessLists on accessListLocations.AccessListId equals accessLists.Id
						join userAccessList in _dbContext.UserAccessLists on accessLists.Id equals userAccessList.AccessListId
						join users in _dbContext.Users on userAccessList.UserId equals users.Id
						where accessListLocations.LocationId == locationId
						select new User(users.Id, users.FirstName, users.LastName);

			var locationDtos = await query.ToListAsync();
			return GetProcessedResult(locationDtos);
		}
	}
}
