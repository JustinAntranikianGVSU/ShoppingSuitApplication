using Domain;
using Domain.Orchestrators;
using Domain.ServiceResult;
using Xunit;

namespace DomainTest
{
	public class ServiceErrorTest
	{
		[Fact]
		public void ServiceError_ConstructorTest()
		{
			var serviceError = new ServiceError("My Error", "My Location");

			Assert.Equal("My Error", serviceError.ErrorMessage);
			Assert.Equal("My Location", serviceError.ErrorLocation);
			Assert.Null(serviceError.FieldName);
		}

		[Fact]
		public void ServiceError_CreateNotSetServiceError()
		{
			var serviceError = ServiceError.CreateNotSetError("FirstName", "UserCommand");

			Assert.Equal("FirstName must be set.", serviceError.ErrorMessage);
			Assert.Equal("UserCommand", serviceError.ErrorLocation);
			Assert.Equal("FirstName", serviceError.FieldName);
		}

		[Fact]
		public void ServiceError_CreateNotSetServiceError_UsingNameof()
		{
			var userInfo = new User();
			var serviceError = ServiceError.CreateNotSetError(nameof(userInfo.FirstName), nameof(CreateUserOrchestrator));

			Assert.Equal("FirstName must be set.", serviceError.ErrorMessage);
			Assert.Equal("CreateUserOrchestrator", serviceError.ErrorLocation);
			Assert.Equal("FirstName", serviceError.FieldName);
		}

		[Fact]
		public void ServiceError_CreateNotSetServiceError_WithTypeof()
		{
			var userInfo = new User();
			var serviceError = ServiceError.CreateNotSetError(nameof(userInfo.FirstName), typeof(CreateUserOrchestrator));

			Assert.Equal("FirstName must be set.", serviceError.ErrorMessage);
			Assert.Equal("CreateUserOrchestrator", serviceError.ErrorLocation);
			Assert.Equal("FirstName", serviceError.FieldName);
		}
	}
}
