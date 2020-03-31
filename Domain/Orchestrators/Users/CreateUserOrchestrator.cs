using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
using Domain.Mappers;
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
		private readonly UsersWithRolesRepository _usersRepository;

		public CreateUserOrchestrator(AppDbContext dbContext) : base(dbContext)
		{
			_usersRepository = new UsersWithRolesRepository(dbContext);
		}

		public async Task<ServiceResult<UserDto>> Create(UserDto userDto)
		{
			var userByEmail = await _usersRepository.GetByEmail(userDto.Email);
			var createUserManager = new CreateUserManager(userDto, userByEmail);
			var errors = createUserManager.GetErrors();
			return errors.Any() ? GetBadRequestResult(errors) : await GetCreateUserResult(createUserManager);
		}

		private async Task<ServiceResult<UserDto>> GetCreateUserResult(CreateUserManager createUserManager)
		{
			var userEntity = createUserManager.CreateEntity();

			await _dbContext.AddAsync(userEntity);
			await _dbContext.SaveChangesAsync();

			var userDto = new UserMapper().Map(userEntity);
			return GetProcessedResult(userDto);
		}
	}
}
