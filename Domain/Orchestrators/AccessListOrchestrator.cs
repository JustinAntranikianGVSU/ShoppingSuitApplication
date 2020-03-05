using DataAccess;
using Domain.Dtos;
using Domain.ServiceResult;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface IAccessListOrchestrator
	{
		Task<ServiceResult<List<AccessListDto>>> Get();

		Task<ServiceResult<AccessListFullDto>> Get(int accessListId);
	}

	public class AccessListOrchestrator : OrchestratorBase<List<AccessListDto>>, IAccessListOrchestrator
	{
		private readonly AppDbContext _dbContext;
		private readonly JwtRequestContext _jwtRequestContext;

		public AccessListOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext)
		{
			(_dbContext, _jwtRequestContext) = (dbContext, jwtRequestContext);
		}

		public async Task<ServiceResult<List<AccessListDto>>> Get()
		{
			var initalQuery = _dbContext.AccessLists.AsNoTracking();
			var clientId = _jwtRequestContext.GetClientId();

			if (clientId.HasValue)
			{
				initalQuery = initalQuery.Where(oo => oo.ClientIdentifier == clientId);
			}

			var accessListEntites = await initalQuery.ToListAsync();
			var accessListDtos = accessListEntites.Select(oo => new AccessListDto(oo.Id, oo.Name));
			return GetProcessedResult(accessListDtos.ToList());
		}

		public async Task<ServiceResult<AccessListFullDto>> Get(int accessListId)
		{
			var accessListEntity = await _dbContext.AccessLists
											.Include(oo => oo.UserAccessListEntities).ThenInclude(oo => oo.User)
											.Include(oo => oo.AccessListLocationEntities).ThenInclude(oo => oo.Location)
											.SingleAsync(oo => oo.Id == accessListId);

			var users = accessListEntity.UserAccessListEntities.Select(oo => new User
			{
				Id = oo.UserId,
				FirstName = oo.User.FirstName,
				LastName = oo.User.LastName,
			});

			var locations = accessListEntity.AccessListLocationEntities.Select(oo => new LocationDto(oo.LocationId, oo.Location.Name));

			var fullDto = new AccessListFullDto(accessListEntity.Id, accessListEntity.Name)
			{
				Locations = locations.ToList(),
				Users = users.ToList()
			};

			return new ServiceResult<AccessListFullDto>(fullDto, ServiceResultStatus.Processed);
		}
	}
}
