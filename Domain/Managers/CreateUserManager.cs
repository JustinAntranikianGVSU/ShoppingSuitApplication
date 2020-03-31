using CoreLibrary;
using CoreLibrary.ServiceResults;
using DataAccess.Entities;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Orchestrators.Users
{
	public class CreateUserManager : ServiceErrorResultBase
	{
		public class CreateResponse
		{
			public List<ServiceError> Errors;
			public UserEntity UserEntity;

			public CreateResponse(List<ServiceError>? errors, UserEntity userEntity)
			{
				Errors = errors ?? new List<ServiceError>();
				UserEntity = userEntity;
			}
		}

		private readonly UserDto _userDto;
		private readonly UserEntity _userByEmail;

		public CreateUserManager(UserDto userDto, UserEntity userByEmail)
		{
			_userDto = userDto;
			_userByEmail = userByEmail;
		}

		public List<ServiceError> GetErrors()
		{
			var fieldsNotSetErrors = GetFieldNotSetErrors();
			var duplicateEmailErrors = GetDuplicateEmailErrors(_userByEmail);
			return fieldsNotSetErrors.Concat(duplicateEmailErrors).ToList();
		}

		public UserEntity CreateEntity()
		{
			var defaultRole = new UserRoleEntity { RoleGuid = RoleLookup.TrainingUserRoleGuid };

			return new UserEntity()
			{
				FirstName = _userDto.FirstName,
				LastName = _userDto.LastName,
				Email = _userDto.Email,
				Roles = new List<UserRoleEntity>() { defaultRole }
			};
		}

		private IEnumerable<ServiceError> GetDuplicateEmailErrors(UserEntity userByEmail)
		{
			if (userByEmail is null)
			{
				yield break;
			}

			var message = $"{userByEmail.Email} is already in use.";
			yield return GetError(message, nameof(userByEmail.Email));
		}

		private IEnumerable<ServiceError> GetFieldNotSetErrors()
		{
			if (string.IsNullOrWhiteSpace(_userDto.FirstName))
			{
				yield return GetNotSetError(nameof(_userDto.FirstName));
			}

			if (string.IsNullOrWhiteSpace(_userDto.LastName))
			{
				yield return GetNotSetError(nameof(_userDto.LastName));
			}

			if (string.IsNullOrWhiteSpace(_userDto.Email))
			{
				yield return GetNotSetError(nameof(_userDto.Email));
			}
		}
	}
}
