using DataAccess;
using Domain.Dtos;
using Domain.Mappers;
using Domain.ServiceResult;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface IAccessListOrchestrator
	{
		Task<ServiceResult<List<AccessListBasicDto>>> Get();

		Task<ServiceResult<AccessListFullDto>> Get(int accessListId);
	}

	public class AccessListOrchestrator : JwtContextOrchestratorBase<List<AccessListBasicDto>>, IAccessListOrchestrator
	{
		public AccessListOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext) : base(dbContext, jwtRequestContext) {}

		public async Task<ServiceResult<List<AccessListBasicDto>>> Get()
		{
			var initalQuery = _dbContext.AccessLists.AsNoTracking();
			var clientId = _jwtRequestContext.GetClientId();

			if (clientId.HasValue)
			{
				initalQuery = initalQuery.Where(oo => oo.ClientIdentifier == clientId);
			}

			var accessListEntites = await initalQuery.ToListAsync();
			var accessListDtos = accessListEntites.Select(oo => new AccessListBasicDto(oo.Id, oo.Name));
			return GetProcessedResult(accessListDtos.ToList());
		}

		/// <summary>
		/// For this method we want to return information about the users and locations associated to this access list.
		/// </summary>
		/// <param name="accessListId"></param>
		/// <returns></returns>
		public async Task<ServiceResult<AccessListFullDto>> Get(int accessListId)
		{
			var accessList = await _dbContext.AccessLists
										.Include(oo => oo.Users).ThenInclude(oo => oo.User)
										.Include(oo => oo.Locations).ThenInclude(oo => oo.Location)
										.AsNoTracking()
										.SingleAsync(oo => oo.Id == accessListId);

			var accessListDto = new AccessListFullDtoMapper().Map(accessList);
			return new ServiceResult<AccessListFullDto>(accessListDto, ServiceResultStatus.Processed);
		}
	}
}
