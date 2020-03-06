using DataAccess.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
	public class UsersRepository : UserBaseRepository
	{
		public UsersRepository(AppDbContext dbContext) : base(dbContext) {}

		public override IQueryable<UserEntity> GetReadOnlyQuery() => _dbContext.Users.AsNoTracking();

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
