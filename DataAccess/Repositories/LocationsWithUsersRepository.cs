using DataAccess.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
	public class LocationsWithUsersRepository : BaseRepository
	{
		public LocationsWithUsersRepository(AppDbContext dbContext) : base(dbContext) {}

		public IQueryable<LocationEntity> GetLocationsWithUsersQuery()
		{
			return _dbContext.Locations
						.AsNoTracking()
						.Include(oo => oo.AccessLists)
						.ThenInclude(oo => oo.AccessList)
						.ThenInclude(oo => oo.Users)
						.ThenInclude(oo => oo.User);
		}
	}
}
