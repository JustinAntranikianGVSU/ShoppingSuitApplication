using DataAccess;
using Domain.Dtos;
using Domain.ServiceResult;
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
			var query = _dbContext.UserAccessLists
							.AsNoTracking()
							.Where(oo => oo.UserId == userId)
							.SelectMany(oo => oo.AccessList.Locations)
							.Select(oo => new LocationBasicDto(oo.Location.Id, oo.Location.Name));

			var locationDtos = await query.ToListAsync();
			return GetProcessedResult(locationDtos);
		}
	}
}
