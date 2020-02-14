using Domain;
using Domain.Orchestrators;
using Domain.ServiceResult;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DomainTest.OrchestratorTests
{
	public class CreateUserOrchestratorTest
	{
		private const string LOCATION = "CreateUserOrchestrator";

		[Fact]
		public async Task CreateUserOrchestrator_Create_ReturnsBadRequest()
		{
			var users = new List<User>();
			var repo = new UserRepository(users);
			var orchestrator = new CreateUserOrchestrator(repo);

			var result = await orchestrator.Create(new User());

			Assert.Equal(3, result.Errors.Count);

			Assert.Collection(result.Errors, (error) => {
				Assert.Equal(LOCATION, error.Location);
				Assert.Equal("FirstName", error.FieldName);
				Assert.Equal("FirstName must be set.", error.Message);
			},
			(error) => {
				Assert.Equal(LOCATION, error.Location);
				Assert.Equal("LastName", error.FieldName);
				Assert.Equal("LastName must be set.", error.Message);
			},
			(error) => {
				Assert.Equal(LOCATION, error.Location);
				Assert.Equal("Email", error.FieldName);
				Assert.Equal("Email must be set.", error.Message);
			});

			Assert.Null(result.Value);
			Assert.Equal(ServiceResultStatus.BadRequest, result.Status);
		}

		[Fact]
		public async Task CreateUserOrchestrator_Create_EmailAlreadyInUse()
		{
			var existingUser = new User { FirstName = "J1", LastName = "A1", Email = "J" };

			var users = new List<User> { existingUser };
			var repo = new UserRepository(users);
			var orchestrator = new CreateUserOrchestrator(repo);

			var newUser = new User { FirstName = "J", LastName = "A", Email = "J" };
			var result = await orchestrator.Create(newUser);

			Assert.Single(result.Errors);

			Assert.Collection(result.Errors, (error) => {
				Assert.Equal(LOCATION, error.Location);
				Assert.Equal("Email", error.FieldName);
				Assert.Equal($"{newUser.Email} is already in use.", error.Message);
			});
		}

		[Fact]
		public async Task CreateUserOrchestrator_Create_AddsUser()
		{
			var existingUser = new User { FirstName = "J1", LastName = "A1", Email = "J1" };

			var users = new List<User> { existingUser };
			var repo = new UserRepository(users);
			var orchestrator = new CreateUserOrchestrator(repo);

			var newUser = new User { FirstName = "J", LastName = "A", Email = "J" };
			var result = await orchestrator.Create(newUser);

			Assert.Null(result.Errors);
			Assert.Equal(100, result.Value.Id);
			Assert.Equal("J", result.Value.FirstName);
			Assert.Equal("A", result.Value.LastName);
			Assert.Equal("J", result.Value.Email);
			Assert.Equal(2, users.Count);
		}
	}
}
