using CoreLibrary.ServiceResults;
using Domain;
using Domain.Orchestrators.Users;
using Xunit;

namespace DomainTest
{
	public class ServiceErrorTest
	{
		private static UserDto CreateUser(int id, string firstName, string lastName, string email = null)
		{
			return new UserDto()
			{
				Id = id,
				FirstName = firstName,
				LastName = lastName,
				Email = email
			};
		}

		[Fact]
		public void ServiceError_ConstructorTest()
		{
			var serviceError = new ServiceError("My Error", "My Location");

			Assert.Equal("My Error", serviceError.Message);
			Assert.Equal("My Location", serviceError.Location);
			Assert.Null(serviceError.FieldName);
		}

		[Fact]
		public void ServiceError_CreateNotSetServiceError()
		{
			var serviceError = ServiceError.CreateNotSetError("FirstName", "UserCommand");

			Assert.Equal("FirstName must be set.", serviceError.Message);
			Assert.Equal("UserCommand", serviceError.Location);
			Assert.Equal("FirstName", serviceError.FieldName);
		}

		[Fact]
		public void ServiceError_CreateNotSetServiceError_UsingNameof()
		{
			var userDto = CreateUser(1, "J", "A");
			var serviceError = ServiceError.CreateNotSetError(nameof(userDto.FirstName), nameof(CreateUserOrchestrator));

			Assert.Equal("FirstName must be set.", serviceError.Message);
			Assert.Equal("CreateUserOrchestrator", serviceError.Location);
			Assert.Equal("FirstName", serviceError.FieldName);
		}

		[Fact]
		public void ServiceError_CreateNotSetServiceError_WithTypeof()
		{
			var userDto = CreateUser(1, "J", "A");
			var serviceError = ServiceError.CreateNotSetError(nameof(userDto.FirstName), typeof(CreateUserOrchestrator));

			Assert.Equal("FirstName must be set.", serviceError.Message);
			Assert.Equal("CreateUserOrchestrator", serviceError.Location);
			Assert.Equal("FirstName", serviceError.FieldName);
		}
	}
}
