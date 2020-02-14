using Domain;
using Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly List<User> _users;

		public UserRepository(List<User> users)
		{
			_users = users;
		}

		public async Task<User> GetByEmail(string email) => _users.FirstOrDefault(oo => oo.Email == email);

		public async Task Add(User user) => _users.Add(user);
	}
}
