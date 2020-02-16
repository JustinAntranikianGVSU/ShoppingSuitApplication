using Domain.ServiceResult;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Orchestrators
{
	public abstract class OrchestratorBase<T> where T : class
	{
		public static string GetResourceNotFoundMessage<TT>(TT id) => $"Could not find User with id: {id}";

		protected ServiceResult<T> GetProcessedResult(T value) => new ServiceResult<T>(value, ServiceResultStatus.Processed);

		protected ServiceResult<T> GetBadRequestResult(List<ServiceError> errors) => new ServiceResult<T>(errors, ServiceResultStatus.BadRequest);

		protected ServiceResult<T> GetNotFoundResult(params ServiceError[] errors) => new ServiceResult<T>(errors.ToList(), ServiceResultStatus.NotFound);

		protected ServiceError GetNotSetError(string fieldName) => ServiceError.CreateNotSetError(fieldName, GetType());

		protected ServiceError GetError(string message, string fieldName) => new ServiceError(message, GetType().Name, fieldName);

		protected ServiceError GetError(string message) => new ServiceError(message, GetType().Name);

	}
}
