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
			accessListEntity.Locations = GetLocations(accessListEntity.Locations, accessListUpdateDto.LocationIds);
			accessListEntity.Users = GetUsers(accessListEntity.Users, accessListUpdateDto.UserIds);

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

		private List<AccessListLocationEntity> GetLocations(ICollection<AccessListLocationEntity> locations, List<int> ids)
		{
			var entitesToKeep = locations.Where(oo => ids.Contains(oo.LocationId));
			var existingIds = locations.Select(oo => oo.LocationId);
			var entitesToAdd = ids.Except(existingIds).Select(oo => new AccessListLocationEntity { LocationId = oo });
			return entitesToKeep.Concat(entitesToAdd).ToList();
		}

		private List<UserAccessListEntity> GetUsers(ICollection<UserAccessListEntity> users, List<int> ids)
		{
			var entitesToKeep = users.Where(oo => ids.Contains(oo.UserId));
			var existingIds = users.Select(oo => oo.UserId);
			var entitesToAdd = ids.Except(existingIds).Select(oo => new UserAccessListEntity { UserId = oo });
			return entitesToKeep.Concat(entitesToAdd).ToList();
		}
	}
}
