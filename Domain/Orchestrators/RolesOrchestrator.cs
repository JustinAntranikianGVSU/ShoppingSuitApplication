using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using System.Collections.Generic;

namespace Domain.Orchestrators
{
	public interface IRolesOrchestrator
	{
		public ServiceResult<List<Role>> GetRoles();
	}

	public class RolesOrchestrator : OrchestratorBase<Role>, IRolesOrchestrator
	{
		public RolesOrchestrator() {}

		public ServiceResult<List<Role>> GetRoles()
		{
			return GetProcessedResult(RoleLookup.Roles);
		}
	}
}
