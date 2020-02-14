
using System;

namespace Domain.ServiceResult
{
	public class ServiceError
	{
		public static ServiceError CreateNotSetError(string field, Type location) => CreateNotSetError(field, location.Name);

		public static ServiceError CreateNotSetError(string fieldName, string errorLocation)
		{
			var message = $"{fieldName} must be set.";
			return new ServiceError(message, errorLocation, fieldName);
		}

		public string ErrorMessage;

		public string ErrorLocation;

		public string? FieldName;

		public ServiceError(string errorMessage, string errorLocation, string? fieldName = null)
		{
			(ErrorMessage, ErrorLocation, FieldName) = (errorMessage, errorLocation, fieldName);
		}
	}
}
