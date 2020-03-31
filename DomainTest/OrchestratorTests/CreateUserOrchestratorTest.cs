using AutoMapper;
using CoreLibrary.ServiceResults;
using DataAccess;
using Domain;
using Domain.Entities;
using Domain.Orchestrators.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DomainTest.OrchestratorTests
{
	public class CreateUserOrchestratorTest
	{
		private const string LOCATION = "CreateUserOrchestrator";

		private static AppDbContext GetInMemoryDb()
		{
			var name = $"new {nameof(AppDbContext)}_{Guid.NewGuid()}";
			var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(name).Options;
			return new AppDbContext(options);
		}

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
		public async Task CreateUserOrchestrator_Create_ReturnsBadRequest()
		{
			var dbContext = GetInMemoryDb();
			var orchestrator = new CreateUserOrchestrator(dbContext);
			var result = await orchestrator.Create(CreateUser(1, "J", "A"));

			Assert.Equal(3, result.Errors.Count);

			Assert.Collection(result.Errors, (error) =>
			{
				Assert.Equal(LOCATION, error.Location);
				Assert.Equal("FirstName", error.FieldName);
				Assert.Equal("FirstName must be set.", error.Message);
			},
			(error) =>
			{
				Assert.Equal(LOCATION, error.Location);
				Assert.Equal("LastName", error.FieldName);
				Assert.Equal("LastName must be set.", error.Message);
			},
			(error) =>
			{
				Assert.Equal(LOCATION, error.Location);
				Assert.Equal("Email", error.FieldName);
				Assert.Equal("Email must be set.", error.Message);
			});

			Assert.Null(result.Value);
			Assert.Equal(ServiceResultStatus.BadRequest, result.Status);
		}

		[Fact]
		public async Task CreateUserOrchestrator_Create_EmailAlreadyInUseError()
		{
			var dbContext = GetInMemoryDb();

			var existingUser = new UserEntity() { FirstName = "J1", LastName = "A1", Email = "E" };
			dbContext.Users.Add(existingUser);
			dbContext.SaveChanges();

			var orchestrator = new CreateUserOrchestrator(dbContext);
			var newUser = CreateUser(1, "J", "A", "E");
			var result = await orchestrator.Create(newUser);

			Assert.Single(result.Errors);

			Assert.Collection(result.Errors, (error) =>
			{
				Assert.Equal(LOCATION, error.Location);
				Assert.Equal("Email", error.FieldName);
				Assert.Equal($"{newUser.Email} is already in use.", error.Message);
			});
		}

		[Fact]
		public async Task CreateUserOrchestrator_Create_AddUserToDbContext()
		{
			var dbContext = GetInMemoryDb();

			var existingUser = new UserEntity() { FirstName = "J1", LastName = "A1", Email = "E1" };
			dbContext.Users.Add(existingUser);
			dbContext.SaveChanges();

			var orchestrator = new CreateUserOrchestrator(dbContext);
			var newUser = CreateUser(1, "J", "A", "E");
			var result = await orchestrator.Create(newUser);

			Assert.Empty(result.Errors);
			Assert.Equal(2, result.Value.Id);
			Assert.Equal("J", result.Value.FirstName);
			Assert.Equal("A", result.Value.LastName);
			Assert.Equal("E", result.Value.Email);

			var userCount = await dbContext.Users.CountAsync();
			Assert.Equal(2, userCount);
		}
	}
}
