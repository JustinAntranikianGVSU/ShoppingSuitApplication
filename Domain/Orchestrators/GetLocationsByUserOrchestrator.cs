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

	public class GetLocationsByUserOrchestrator : OrchestratorBase<List<LocationBasicDto>>, IGetLocationsByUserOrchestrator
	{
		private readonly AppDbContext _dbContext;
		private readonly JwtRequestContext _jwtRequestContext;

		public GetLocationsByUserOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext)
		{
			(_dbContext, _jwtRequestContext) = (dbContext, jwtRequestContext);
		}

		public async Task<ServiceResult<List<LocationBasicDto>>> Get() => await Get(_jwtRequestContext.GetUserId());

		public async Task<ServiceResult<List<LocationBasicDto>>> Get(int userId)
		{
			var query = from userLists in _dbContext.UserAccessLists
						join listLocations in _dbContext.AccessListLocations on userLists.AccessListId equals listLocations.AccessListId
						join locations in _dbContext.Locations on listLocations.LocationId equals locations.Id
						where userLists.UserId == userId
						select new LocationBasicDto(locations.Id, locations.Name);

			var locationDtos = await query.ToListAsync();
			return GetProcessedResult(locationDtos);
		}
	}
}
