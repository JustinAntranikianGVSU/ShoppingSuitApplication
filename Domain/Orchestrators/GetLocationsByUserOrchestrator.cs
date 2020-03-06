using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Entities;
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
		private readonly UsersRepository _usersRepository;

		public GetLocationsByUserOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext) : base(dbContext, jwtRequestContext)
		{
			_usersRepository = new UsersRepository(_dbContext);
		}

		public async Task<ServiceResult<List<LocationBasicDto>>> Get() => await Get(_jwtRequestContext.GetUserId());

		public async Task<ServiceResult<List<LocationBasicDto>>> Get(int userId)
		{
			var locationDtos = await _usersRepository.GetLocations(userId).Select(oo => new LocationBasicDto(oo.Id, oo.Name)).ToListAsync();
			return GetProcessedResult(locationDtos);
		}
	}
}
