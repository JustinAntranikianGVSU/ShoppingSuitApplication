using CoreLibrary;
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

namespace Domain.Orchestrators
{
	public interface IAccessListOrchestrator
	{
		Task<ServiceResult<List<AccessListBasicDto>>> Get();

		Task<ServiceResult<AccessListFullDto>> Get(int accessListId);
	}

	public class AccessListOrchestrator : JwtContextOrchestratorBase<AccessListBasicDto>, IAccessListOrchestrator
	{
		private readonly AccessListFullDtoMapper _accessListFullDtoMapper;
		private readonly AccessListRepository _accessListRepository;

		public AccessListOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext) : base(dbContext, jwtRequestContext)
		{
			_accessListFullDtoMapper = new AccessListFullDtoMapper();
			_accessListRepository = new AccessListRepository(dbContext);
		}

		public async Task<ServiceResult<List<AccessListBasicDto>>> Get()
		{
			var accessListDtos = await GetAccessListQuery().Select(oo => new AccessListBasicDto(oo.Id, oo.Name)).ToListAsync();
			return GetProcessedResult(accessListDtos);
		}

		private IQueryable<AccessListEntity> GetAccessListQuery()
		{
			var clientId = _jwtRequestContext.GetClientId();
			var queryable = _accessListRepository.GetReadOnlyQuery();
			return clientId.HasValue ? queryable.Where(oo => oo.ClientIdentifier == clientId.Value) : queryable;
		}

		/// <summary>
		/// For this method we want to return information about the users and locations associated to this access list.
		/// </summary>
		/// <param name="accessListId"></param>
		/// <returns></returns>
		public async Task<ServiceResult<AccessListFullDto>> Get(int accessListId)
		{
			var accessList = await _accessListRepository.GetFullAccessList(accessListId);
			var accessListDto = _accessListFullDtoMapper.Map(accessList);
			return new ServiceResult<AccessListFullDto>(accessListDto, ServiceResultStatus.Processed);
		}
	}
}
