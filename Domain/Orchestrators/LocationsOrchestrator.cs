using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface ILocationsOrchestrator
	{
		Task<ServiceResult<List<LocationWithUsersDto>>> GetAll();
	}

	public class LocationsOrchestrator : OrchestratorBase, ILocationsOrchestrator
	{
		private readonly LocationsWithUsersRepository _locationsRepository;

		public LocationsOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext) : base(dbContext, jwtRequestContext)
		{
			_locationsRepository = new LocationsWithUsersRepository(_dbContext);
		}

		public async Task<ServiceResult<List<LocationWithUsersDto>>> GetAll()
		{
			var clientId = _jwtRequestContext.GetClientId();
			var locationEntities = await _locationsRepository.GetAll(clientId);
			var locationDtos = new LocationWithAccessListMapper().Map(locationEntities);
			return GetProcessedResult(locationDtos);
		}
	}
}
