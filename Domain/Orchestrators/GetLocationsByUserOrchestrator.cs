using CoreLibrary.Orchestrators;
using CoreLibrary.RequestContexts;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
using Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface IGetLocationsByUserOrchestrator
	{
		/// <summary>
		/// Gets the locations for the logged in user (impersonation takes priority).
		/// </summary>
		/// <returns></returns>
		Task<ServiceResult<List<LocationBasicDto>>> Get();

		Task<ServiceResult<List<LocationBasicDto>>> Get(int userId);
	}

	public class GetLocationsByUserOrchestrator : JwtContextOrchestratorBase<List<LocationBasicDto>>, IGetLocationsByUserOrchestrator
	{
		public GetLocationsByUserOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext) : base(dbContext, jwtRequestContext) {}

		public async Task<ServiceResult<List<LocationBasicDto>>> Get() => await Get(_jwtRequestContext.GetUserId());

		public async Task<ServiceResult<List<LocationBasicDto>>> Get(int userId)
		{
			var query = new UsersRepository(_dbContext).GetLocations(userId);
			var locationDtos = await query.Select(oo => new LocationBasicDto(oo.Id, oo.Name)).ToListAsync();
			return GetProcessedResult(locationDtos);
		}
	}
}
