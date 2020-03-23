using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface IAccessListOrchestrator
	{
		Task<ServiceResult<List<AccessListDto>>> GetAll();

		Task<ServiceResult<AccessListDto>> Get(int accessListId);
	}

	public class AccessListOrchestrator : JwtContextOrchestratorBase<AccessListDto>, IAccessListOrchestrator
	{
		private readonly AccessListDtoMapper _accessListFullDtoMapper;
		private readonly AccessListRepository _accessListRepository;

		public AccessListOrchestrator(AppDbContext dbContext, JwtRequestContext jwtRequestContext) : base(dbContext, jwtRequestContext)
		{
			_accessListFullDtoMapper = new AccessListDtoMapper();
			_accessListRepository = new AccessListRepository(dbContext);
		}

		public async Task<ServiceResult<List<AccessListDto>>> GetAll()
		{
			var clientId = _jwtRequestContext.GetClientId();
			var accessListEntities = await _accessListRepository.GetAll(clientId);
			var accessListDtos = _accessListFullDtoMapper.Map(accessListEntities);
			return GetProcessedResult(accessListDtos);
		}

		public async Task<ServiceResult<AccessListDto>> Get(int accessListId)
		{
			var accessList = await _accessListRepository.SingleAsync(accessListId);
			var accessListDto = _accessListFullDtoMapper.Map(accessList);
			return GetProcessedResult(accessListDto);
		}
	}
}
