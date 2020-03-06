using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators.Locations
{
	public interface IGetUsersByLocationOrchestrator
	{
		Task<ServiceResult<List<UserBasicDto>>> Get(int id);
	}

	public class GetUsersByLocationOrchestrator : DbContextOrchestratorBase<List<UserBasicDto>>, IGetUsersByLocationOrchestrator
	{
		private readonly LocationsRepository _locationsRepository;

		public GetUsersByLocationOrchestrator(AppDbContext dbContext) : base(dbContext)
		{
			_locationsRepository = new LocationsRepository(_dbContext);
		}

		/// <summary>
		/// Gets a list of users assocated to a given location by going through the accessLists.
		/// </summary>
		/// <param name="locationId"></param>
		/// <returns></returns>
		public async Task<ServiceResult<List<UserBasicDto>>> Get(int locationId)
		{
			var query = _locationsRepository.GetUsers(locationId);
			var userDtos = await query.Select(oo => new UserBasicDto(oo.Id, oo.FirstName, oo.LastName)).ToListAsync();
			return GetProcessedResult(userDtos);
		}
	}
}
