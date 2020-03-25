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

namespace Domain.Orchestrators.Users
{
	public interface IUpdateAccessListOrchestrator
	{
		public Task<ServiceResult<AccessListDto>> Update(int id, AccessListUpdateDto accessListUpdateDto);
	}

	public class UpdateAccessListOrchestrator : OrchestratorBase, IUpdateAccessListOrchestrator
	{
		private readonly AccessListDtoMapper _accessListMapper;
		private readonly AccessListRepository _accessListRepository;

		public UpdateAccessListOrchestrator(AppDbContext dbContext) : base(dbContext)
		{
			_accessListMapper = new AccessListDtoMapper();
			_accessListRepository = new AccessListRepository(dbContext);
		}

		public async Task<ServiceResult<AccessListDto>> Update(int id, AccessListUpdateDto accessListUpdateDto)
		{
			var accessListEntity = await GetAccessListBasicQuery().SingleAsync(oo => oo.Id == id);

			accessListEntity.Name = accessListUpdateDto.Name;
			accessListEntity.Locations = GetLocations(accessListEntity, accessListUpdateDto.LocationIds);
			accessListEntity.Users = GetUsers(accessListEntity, accessListUpdateDto.UserIds);

			await _dbContext.SaveChangesAsync();

			var result = await GetAccessListDto(id);
			return GetProcessedResult(result);
		}

		private IQueryable<AccessListEntity> GetAccessListBasicQuery()
		{
			return _dbContext.AccessLists
						.Include(oo => oo.Locations)
						.Include(oo => oo.Users);
		}

		private async Task<AccessListDto> GetAccessListDto(int id)
		{
			var accessListEntity = await _accessListRepository.SingleAsync(id);
			return _accessListMapper.Map(accessListEntity);
		}

		private List<AccessListLocationEntity> GetLocations(AccessListEntity accessListEntity, List<int> newLocationIds)
		{
			var entitesToKeep = accessListEntity.Locations.Where(oo => newLocationIds.Contains(oo.LocationId));
			var existingIds = accessListEntity.Locations.Select(oo => oo.LocationId);
			var entitesToAdd = newLocationIds.Except(existingIds).Select(oo => new AccessListLocationEntity { LocationId = oo });
			return entitesToKeep.Concat(entitesToAdd).ToList();
		}

		private List<UserAccessListEntity> GetUsers(AccessListEntity accessListEntity, List<int> newUsersIds)
		{
			var entitesToKeep = accessListEntity.Users.Where(oo => newUsersIds.Contains(oo.UserId));
			var existingIds = accessListEntity.Users.Select(oo => oo.UserId);
			var entitesToAdd = newUsersIds.Except(existingIds).Select(oo => new UserAccessListEntity { UserId = oo });
			return entitesToKeep.Concat(entitesToAdd).ToList();
		}
	}
}
