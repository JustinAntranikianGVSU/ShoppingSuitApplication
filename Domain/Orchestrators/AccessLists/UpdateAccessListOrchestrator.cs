using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Mappers;
using Domain.Orchestrators.AccessLists;
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
			var updateResult = new UpdateAccessListManager().GetResult(accessListEntity, accessListUpdateDto);

			accessListEntity.Name = updateResult.Name;
			accessListEntity.Locations = updateResult.Locations;
			accessListEntity.Users = updateResult.Users;

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
	}
}
