using CoreLibrary.ServiceResults;
using System.Linq;

namespace CoreLibrary.Orchestrators
{
	public abstract class OrchestratorBase<T> where T : class
	{
		public static string GetResourceNotFoundMessage<TT>(TT id) => $"Could not find User with id: {id}";

		protected ServiceResult<T> GetProcessedResult(T value) => new ServiceResult<T>(value, ServiceResultStatus.Processed);

		protected ServiceResult<T> GetBadRequestResult(string errorMessage)
		{
			var errors = new[] { GetError(errorMessage) };
			return new ServiceResult<T>(errors.ToList(), ServiceResultStatus.BadRequest);
		}

		protected ServiceResult<T> GetBadRequestResult(params ServiceError[] errors) => new ServiceResult<T>(errors.ToList(), ServiceResultStatus.BadRequest);

		protected ServiceResult<T> GetNotFoundResult(params ServiceError[] errors) => new ServiceResult<T>(errors.ToList(), ServiceResultStatus.NotFound);

		protected ServiceResult<T> GetNotFoundResult(string errorMessage)
		{
			var errors = new[] { GetError(errorMessage) };
			return new ServiceResult<T>(errors.ToList(), ServiceResultStatus.NotFound);
		}

		protected ServiceError GetNotSetError(string fieldName) => ServiceError.CreateNotSetError(fieldName, GetType());

		protected ServiceError GetError(string message, string fieldName) => new ServiceError(message, GetType().Name, fieldName);

		protected ServiceError GetError(string message) => new ServiceError(message, GetType().Name);
	}
}
