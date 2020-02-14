using Domain;
using Domain.Orchestrators;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DomainTest.OrchestratorTests
{
	public class CreateUserOrchestratorTest
	{
		[Fact]
		public async Task CreateUserOrchestrator_Create_ReturnsEmptyFieldErrors()
		{
			var users = new List<User>();
			var repo = new UserRepository(users);
			var orchestrator = new CreateUserOrchestrator(repo);

			var result = await orchestrator.Create(new User());

			Assert.Equal(2, result.Errors.Count);
		}
	}
}
