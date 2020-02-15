using AutoMapper;
using DataAccess;
using Domain;
using Domain.Entities;
using Domain.Orchestrators;
using Domain.ServiceResult;
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

		private static IMapper GetMapper()
		{
			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});

			return mappingConfig.CreateMapper();
		}

		[Fact]
		public async Task CreateUserOrchestrator_Create_ReturnsBadRequest()
		{
			var dbContext = GetInMemoryDb();
			var orchestrator = new CreateUserOrchestrator(dbContext, GetMapper());
			var result = await orchestrator.Create(new User());

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

			var existingUser = new UserEntity() { FirstName = "J1", LastName = "A1", Email = "J" };
			dbContext.Users.Add(existingUser);
			dbContext.SaveChanges();

			var orchestrator = new CreateUserOrchestrator(dbContext, GetMapper());
			var newUser = new User("J", "A", "J");
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

			var existingUser = new UserEntity() { FirstName = "J1", LastName = "A1", Email = "J1" };
			dbContext.Users.Add(existingUser);
			dbContext.SaveChanges();

			var orchestrator = new CreateUserOrchestrator(dbContext, GetMapper());
			var newUser = new User("J", "A", "J");
			var result = await orchestrator.Create(newUser);

			Assert.Empty(result.Errors);
			Assert.Equal(2, result.Value.Id);
			Assert.Equal("J", result.Value.FirstName);
			Assert.Equal("A", result.Value.LastName);
			Assert.Equal("J", result.Value.Email);

			var userCount = await dbContext.Users.CountAsync();
			Assert.Equal(2, userCount);
		}
	}
}
