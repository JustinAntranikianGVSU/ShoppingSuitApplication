using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
	public class UsersWithLocationsRepository : UserRepository
	{
		public UsersWithLocationsRepository(AppDbContext dbContext) : base(dbContext) { }

		public override IQueryable<UserEntity> GetReadOnlyQuery()
		{
			return _dbContext.Users
						.AsNoTracking()
						.Include(oo => oo.AccessLists)
						.ThenInclude(oo => oo.AccessList)
						.ThenInclude(oo => oo.Locations)
						.ThenInclude(oo => oo.Location)
						.Include(oo => oo.Roles);
		}
	}
}
