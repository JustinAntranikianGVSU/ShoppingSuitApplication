using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class LocationsWithUsersRepository : BaseRepository
	{
		public LocationsWithUsersRepository(AppDbContext dbContext) : base(dbContext) {}

		private IQueryable<LocationEntity> GetReadOnlyQuery()
		{
			return _dbContext.Locations
						.AsNoTracking()
						.Include(oo => oo.AccessLists)
						.ThenInclude(oo => oo.AccessList)
						.ThenInclude(oo => oo.Users)
						.ThenInclude(oo => oo.User);
		}

		public async Task<List<LocationEntity>> GetAll(Guid? clientId)
		{
			var queryable = GetReadOnlyQuery();
			var withClientQueryable = clientId.HasValue ? queryable.Where(oo => oo.ClientIdentifier == clientId.Value) : queryable;
			return await withClientQueryable.ToListAsync();
		}
	}
}
