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

		public async Task<UserEntity> GetByEmail(string email) => await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(oo => oo.Email == email);

		public IQueryable<LocationEntity> GetLocations(int userId)
		{
			return _dbContext.UserAccessLists
						.AsNoTracking()
						.Where(oo => oo.UserId == userId)
						.SelectMany(oo => oo.AccessList.Locations)
						.Select(oo => oo.Location);
		}
	}
}
