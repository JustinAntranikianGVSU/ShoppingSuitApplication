using CoreLibrary.Orchestrators;
using CoreLibrary.RequestContexts;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Entities;
using Domain.Dtos;
using Domain.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface IGetLocationsOrchestrator
	{
		Task<ServiceResult<List<LocationWithAccessListDto>>> Get();
	}

	public class GetLocationsOrchestrator : JwtContextOrchestratorBase<List<LocationWithAccessListDto>>, IGetLocationsOrchestrator
	{
		public GetLocationsOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext) : base(dbContext, jwtRequestContext) {}

		public async Task<ServiceResult<List<LocationWithAccessListDto>>> Get()
		{
			var locationEntities = await GetQueryable().ToListAsync();
			var locationWithAccessListDtos = new LocationWithAccessListMapper().Map(locationEntities);
			return GetProcessedResult(locationWithAccessListDtos);
		}

		private IQueryable<LocationEntity> GetQueryable()
		{
			var initalQuery = _dbContext.Locations.Include(oo => oo.AccessLists).ThenInclude(oo => oo.AccessList).AsNoTracking();
			var clientId = _jwtRequestContext.GetClientId();
			return clientId.HasValue ? initalQuery.Where(oo => oo.ClientIdentifier == clientId) : initalQuery;
		}
	}
}
