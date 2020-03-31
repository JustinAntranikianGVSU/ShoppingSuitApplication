namespace CoreLibrary.ServiceResults
{
	public class ServiceErrorResultBase
	{
		protected ServiceError GetNotSetError(string fieldName) => ServiceError.CreateNotSetError(fieldName, GetType());

		protected ServiceError GetError(string message, string fieldName) => new ServiceError(message, GetType().Name, fieldName);

		protected ServiceError GetError(string message) => new ServiceError(message, GetType().Name);

		public static string GetResourceNotFoundMessage<T>(T id) where T : struct => $"Could not find User with id: {id}";
	}
}
