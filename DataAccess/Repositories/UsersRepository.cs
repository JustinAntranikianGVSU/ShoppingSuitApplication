using DataAccess.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class UsersRepository : BaseRepository
	{
		public UsersRepository(AppDbContext dbContext) : base(dbContext) {}

		public IQueryable<LocationEntity> GetLocations(int userId)
		{
			return _dbContext.UserAccessLists
						.AsNoTracking()
						.Where(oo => oo.UserId == userId)
						.SelectMany(oo => oo.AccessList.Locations)
						.Select(oo => oo.Location);
		}

		public IQueryable<UserEntity> GetRolesQuery() => _dbContext.Users.Include(oo => oo.Roles).AsNoTracking();

		public async Task<UserEntity> SingleAsync(int userId)
		{
			return await GetRolesQuery().SingleAsync(oo => oo.Id == userId);
		}

		public async Task<UserEntity?> SingleOrDefaultAsync(int userId)
		{
			return await GetRolesQuery().SingleOrDefaultAsync(oo => oo.Id == userId);
		}
	}
}
