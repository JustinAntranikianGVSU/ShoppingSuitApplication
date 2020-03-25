using AutoMapper;
using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using Domain.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Orchestrators.Users
{
	public interface ICreateUserOrchestrator
	{
		Task<ServiceResult<UserDto>> Create(UserDto user);
	}

	public class CreateUserOrchestrator : OrchestratorBase<UserDto>, ICreateUserOrchestrator
	{
		private readonly UserMapper _userMapper;
		private readonly UsersWithRolesRepository _usersRepository;

		public CreateUserOrchestrator(AppDbContext dbContext, IMapper mapper) : base(dbContext)
		{
			_userMapper = new UserMapper(mapper);
			_usersRepository = new UsersWithRolesRepository(dbContext);
		}

		public async Task<ServiceResult<UserDto>> Create(UserDto user)
		{
			var errors = GetServiceErrors(user).ToList();
			var userByEmail = await _usersRepository.GetByEmail(user.Email);

			if (userByEmail is { Email: var email })
			{
				var message = $"{email} is already in use.";
				var error = GetError(message, nameof(email));
				errors.Add(error);
			}

			if (errors.Any())
			{
				return GetBadRequestResult(errors.ToArray());
			}

			var userEntity = _userMapper.Map(user);
			userEntity.Roles.Add(new UserRoleEntity { RoleGuid = RoleLookup.TrainingUserRoleGuid });

			await _dbContext.AddAsync(userEntity);
			await _dbContext.SaveChangesAsync();

			var newUser = _userMapper.Map(userEntity);
			return GetProcessedResult(newUser);
		}

		private IEnumerable<ServiceError> GetServiceErrors(UserDto user)
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
