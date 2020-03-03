using DataAccess;
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
		Task<ServiceResult<List<LocationDto>>> Get();
	}

	public class GetLocationsOrchestrator : OrchestratorBase<List<LocationDto>>, IGetLocationsOrchestrator
	{
		private readonly AppDbContext _dbContext;

		public GetLocationsOrchestrator(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<ServiceResult<List<LocationDto>>> Get()
		{
			var locationDtos = await _dbContext.Locations.Select(oo => new LocationDto(oo.Id, oo.Name)).ToListAsync();
			return GetProcessedResult(locationDtos);
		}
	}
}
