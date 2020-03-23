using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class UsersWithLocationsRepository : BaseRepository
	{
		public UsersWithLocationsRepository(AppDbContext dbContext) : base(dbContext) { }

		private IQueryable<UserEntity> GetReadOnlyQuery()
		{
			return _dbContext.Users
						.AsNoTracking()
						.Include(oo => oo.Roles)
						.Include(oo => oo.AccessLists)
						.ThenInclude(oo => oo.AccessList)
						.ThenInclude(oo => oo.Locations)
						.ThenInclude(oo => oo.Location);
		}

		public async Task<UserEntity> SingleAsync(int userId)
		{
			return await GetReadOnlyQuery().SingleAsync(oo => oo.Id == userId);
		}

		public async Task<List<UserEntity>> GetAll(Guid? clientId)
		{
			var queryable = GetReadOnlyQuery();
			var withClientQueryable = clientId.HasValue ? queryable.Where(oo => oo.ClientIdentifier == clientId.Value) : queryable;
			return await withClientQueryable.ToListAsync();
		}
	}
}
