using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
	public interface IUserRepository
	{
		public Task<User?> GetByEmail(string email);

		public Task Add(User user);
	}
}
