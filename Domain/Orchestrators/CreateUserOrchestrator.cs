using DataAccess;
using DataAccess.Entities;
using Domain.Entities;
using Domain.Security;
using Domain.ServiceResult;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators
{
	public interface ICreateUserOrchestrator
	{
		Task<ServiceResult<User>> Create(User user);
	}

	public class CreateUserOrchestrator : OrchestratorBase<User>, ICreateUserOrchestrator
	{
		private readonly AppDbContext _dbContext;

		public CreateUserOrchestrator(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<ServiceResult<User>> Create(User user)
		{
			var errors = GetServiceErrors(user).ToList();
			var userByEmail = await _dbContext.Users.FirstOrDefaultAsync(oo => oo.Email == user.Email);

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

			var userEntity = new UserEntity
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Roles = new List<UserRoleEntity> { new UserRoleEntity { RoleGuid = RoleLookup.TrainingUserRoleGuid } }
			};

			_dbContext.Users.Add(userEntity);
			await _dbContext.SaveChangesAsync();

			var newUserDto = new User
			{
				Id = userEntity.Id,
				FirstName = userEntity.FirstName,
				LastName = userEntity.LastName,
				Email = userEntity.Email
			};

			return GetProcessedResult(newUserDto);
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
