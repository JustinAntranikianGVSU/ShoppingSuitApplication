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
				var error = new ServiceError(message, nameof(userByEmail.Email), nameof(CreateUserOrchestrator));
				errors.Add(error);
			}

			var result = errors.Any() ? GetBadRequestResult(errors) : GetProcessedResult(user);
			return result;
		}

		private IEnumerable<ServiceError> GetServiceErrors(User user)
		{
			if (string.IsNullOrWhiteSpace(user.FirstName))
			{
				var error = ServiceError.CreateNotSetError(nameof(user.FirstName), GetType());
				yield return error;
			}

			if (string.IsNullOrWhiteSpace(user.LastName))
			{
				var error = ServiceError.CreateNotSetError(nameof(user.LastName), GetType());
				yield return error;
			}
		}
	}
}
