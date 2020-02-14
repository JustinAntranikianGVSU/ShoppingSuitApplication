using Domain.RepositoryInterfaces;
using Domain.ServiceResult;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public class CreateUserOrchestrator : OrchestratorBase<User>
	{
		private readonly IUserRepository _repo;

		public CreateUserOrchestrator(IUserRepository repo)
		{
			_repo = repo;
		}

		public async Task<ServiceResult<User>> Create(User user)
		{
			var errors = GetServiceErrors(user).ToList();
			var userByEmail = await _repo.GetByEmail(user.Email);

			if (userByEmail is {})
			{
				var message = $"{userByEmail.Email} is already in use.";
				var error = GetError(message, nameof(user.Email));
				errors.Add(error);
			}

			if (errors.Any())
			{
				return GetBadRequestResult(errors);
			}

			var userToCreate = new User
			{
				Id = 100,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				DateOfBirth = user.DateOfBirth
			};

			await _repo.Add(userToCreate);
			return GetProcessedResult(userToCreate);
		}

		private IEnumerable<ServiceError> GetServiceErrors(User user)
		{
			if (string.IsNullOrWhiteSpace(user.FirstName))
			{
				yield return GetNotSetError(nameof(user.FirstName));
			}

			if (string.IsNullOrWhiteSpace(user.LastName))
			{
				yield return GetNotSetError(nameof(user.LastName));
			}

			if (string.IsNullOrWhiteSpace(user.Email))
			{
				yield return GetNotSetError(nameof(user.Email));
			}
		}
	}
}
