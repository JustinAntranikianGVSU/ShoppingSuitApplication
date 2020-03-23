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

namespace Domain.Orchestrators
{
	public interface IGetLocationsOrchestrator
	{
		Task<ServiceResult<List<LocationWithUsersDto>>> GetAll();
	}

	public class GetLocationsOrchestrator : JwtContextOrchestratorBase<List<LocationWithUsersDto>>, IGetLocationsOrchestrator
	{
		private readonly LocationWithAccessListMapper _locationWithAccessListMapper;
		private readonly LocationsWithUsersRepository _locationsRepository;

		public GetLocationsOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext) : base(dbContext, jwtRequestContext)
		{
			_locationWithAccessListMapper = new LocationWithAccessListMapper();
			_locationsRepository = new LocationsWithUsersRepository(_dbContext);
		}

		public async Task<ServiceResult<List<LocationWithUsersDto>>> GetAll()
		{
			var locationEntities = await GetQuery().ToListAsync();
			var locationDtos = _locationWithAccessListMapper.Map(locationEntities);
			return GetProcessedResult(locationDtos);
		}

		private IQueryable<LocationEntity> GetQuery()
		{
			var queryable = _locationsRepository.GetLocationsWithUsersQuery();
			var clientId = _jwtRequestContext.GetClientId();
			return clientId.HasValue ? queryable.Where(oo => oo.ClientIdentifier == clientId) : queryable;
		}
	}
}
