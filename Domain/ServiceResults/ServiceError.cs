
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

		/// <summary>
		/// Provide a custom message to send back to the user.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Usually the type name of the Orchestrator that validated the error.
		/// </summary>
		public string Location { get; set; }

		/// <summary>
		/// Name of the field where the error occured (ie. FirstName, Email, etc)
		/// </summary>
		public string? FieldName { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message">Provide a custom message to send back to the user.</param>
		/// <param name="location">Usually the type name of the Orchestrator that validated the error.</param>
		/// <param name="fieldName">Name of the field where the error occured (ie. FirstName, Email, etc).</param>
		public ServiceError(string message, string location, string? fieldName = null)
		{
			(Message, Location, FieldName) = (message, location, fieldName);
		}
	}
}
