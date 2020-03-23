using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class UserRepository : BaseRepository
	{
		protected UserRepository(AppDbContext dbContext) : base(dbContext) { }

		protected virtual IQueryable<UserEntity> GetReadOnlyQuery() => _dbContext.Users.AsNoTracking();
		
		public async Task<UserEntity> GetByEmail(string email) => await GetReadOnlyQuery().FirstOrDefaultAsync(oo => oo.Email == email);

		public async Task<UserEntity> SingleAsync(int userId) => await GetReadOnlyQuery().SingleAsync(oo => oo.Id == userId);

		public async Task<UserEntity?> SingleOrDefaultAsync(int userId) => await GetReadOnlyQuery().SingleOrDefaultAsync(oo => oo.Id == userId);
	}
}
