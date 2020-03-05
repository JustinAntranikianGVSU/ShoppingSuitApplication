using AutoMapper;
using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Entities;
using Domain.Entities;
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
		private readonly IMapper _mapper;

		public CreateUserOrchestrator(AppDbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ServiceResult<User>> Create(User user)
		{
			var errors = GetServiceErrors(user).ToList();
			var userByEmail = await _dbContext.Users.GetByEmail(user.Email);

			if (userByEmail is {})
			{
				var message = $"{userByEmail.Email} is already in use.";
				var error = GetError(message, nameof(user.Email));
				errors.Add(error);
			}

			if (errors.Any())
			{
				return GetBadRequestResult(errors.ToArray());
			}

			var userEntity = _mapper.Map<UserEntity>(user);
			userEntity.Roles.Add(new UserRoleEntity { RoleGuid = RoleLookup.TrainingUserRoleGuid });

			await _dbContext.AddAsync(userEntity);
			await _dbContext.SaveChangesAsync();

			var newUser = _mapper.Map<User>(userEntity);
			return GetProcessedResult(newUser);
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
