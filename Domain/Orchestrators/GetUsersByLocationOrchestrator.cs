using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface IGetUsersByLocationOrchestrator
	{
		Task<ServiceResult<List<UserDto>>> Get(int id);
	}

	public class GetUsersByLocationOrchestrator : DbContextOrchestratorBase<List<UserDto>>, IGetUsersByLocationOrchestrator
	{
		public GetUsersByLocationOrchestrator(AppDbContext dbContext) : base(dbContext) {}

		/// <summary>
		/// Gets a list of users assocated to a given location by going through the accessLists.
		/// </summary>
		/// <param name="locationId"></param>
		/// <returns></returns>
		public async Task<ServiceResult<List<UserDto>>> Get(int locationId)
		{
			var query = new LocationsRepository(_dbContext).GetUsers(locationId);
			var userDtos = await query.Select(oo => new UserDto(oo.Id, oo.FirstName, oo.LastName)).ToListAsync();
			return GetProcessedResult(userDtos);
		}
	}
}
