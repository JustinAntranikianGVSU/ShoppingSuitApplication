using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public abstract class UserBaseRepository : BaseRepository
	{
		protected UserBaseRepository(AppDbContext dbContext) : base(dbContext) { }

		public abstract IQueryable<UserEntity> GetReadOnlyQuery();

		public async Task<UserEntity> GetByEmail(string email) => await GetReadOnlyQuery().FirstOrDefaultAsync(oo => oo.Email == email);
	}
}
