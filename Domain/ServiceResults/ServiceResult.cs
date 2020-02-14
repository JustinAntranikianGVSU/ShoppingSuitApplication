using System.Collections.Generic;

namespace Domain.ServiceResult
{
	public class ServiceResult<T> where T : class
	{
		public T? Value;

		public ServiceResultStatus Status;

		public List<ServiceError>? Errors;

		public ServiceResult(List<ServiceError> errors, ServiceResultStatus status)
		{
			(Status, Errors) = (status, errors);
		}

		public ServiceResult(T value, ServiceResultStatus status)
		{
			(Value, Status) = (value, status);
		}
	}
}
