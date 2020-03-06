using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators.Locations
{
	public interface IGetLocationsOrchestrator
	{
		Task<ServiceResult<List<LocationWithAccessListDto>>> Get();
	}

	public class GetLocationsOrchestrator : JwtContextOrchestratorBase<List<LocationWithAccessListDto>>, IGetLocationsOrchestrator
	{
		private readonly LocationsRepository _locationsRepository;

		public GetLocationsOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext) : base(dbContext, jwtRequestContext)
		{
			_locationsRepository = new LocationsRepository(_dbContext);
		}

		public async Task<ServiceResult<List<LocationWithAccessListDto>>> Get()
		{
			var locationEntities = await GetQuery().ToListAsync();
			var locationWithAccessListDtos = new LocationWithAccessListMapper().Map(locationEntities);
			return GetProcessedResult(locationWithAccessListDtos);
		}

		private IQueryable<LocationEntity> GetQuery()
		{
			var queryable = _locationsRepository.GetLocationsWithAccessListsQuery();
			var clientId = _jwtRequestContext.GetClientId();
			return clientId.HasValue ? queryable.Where(oo => oo.ClientIdentifier == clientId) : queryable;
		}
	}
}
