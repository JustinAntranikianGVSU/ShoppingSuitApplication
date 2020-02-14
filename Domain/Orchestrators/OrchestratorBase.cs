using Domain.ServiceResult;
using System.Collections.Generic;

namespace Domain.Orchestrators
{
	public abstract class OrchestratorBase<T> where T : class
	{
		protected ServiceResult<T> GetProcessedResult(T value) => new ServiceResult<T>(value, ServiceResultStatus.Processed);

		protected ServiceResult<T> GetBadRequestResult(List<ServiceError> errors) => new ServiceResult<T>(errors, ServiceResultStatus.BadRequest);
	}
}
