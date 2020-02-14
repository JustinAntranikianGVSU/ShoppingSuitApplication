using Domain.ServiceResult;
using System.Collections.Generic;

namespace Domain.Orchestrators
{
	public abstract class OrchestratorBase<T> where T : class
	{
		protected ServiceResult<T> GetProcessedResult(T value) => new ServiceResult<T>(value, ServiceResultStatus.Processed);

		protected ServiceResult<T> GetBadRequestResult(List<ServiceError> errors) => new ServiceResult<T>(errors, ServiceResultStatus.BadRequest);

		protected ServiceError GetNotSetError(string fieldName) => ServiceError.CreateNotSetError(fieldName, GetType());

		protected ServiceError GetError(string message, string fieldName) => new ServiceError(message, GetType().Name, fieldName);

		protected ServiceError GetError(string message) => new ServiceError(message, GetType().Name);
	}
}
