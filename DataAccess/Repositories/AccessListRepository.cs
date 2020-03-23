using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class AccessListRepository : BaseRepository
	{
		public AccessListRepository(AppDbContext dbContext) : base(dbContext) { }

		private IQueryable<AccessListEntity> GetReadOnlyQuery()
		{
			return _dbContext.AccessLists
						.AsNoTracking()
						.Include(oo => oo.Users)
						.ThenInclude(oo => oo.User)
						.Include(oo => oo.Locations)
						.ThenInclude(oo => oo.Location);
		}

		public async Task<AccessListEntity> SingleAsync(int accessListId)
		{
			return await GetReadOnlyQuery().SingleAsync(oo => oo.Id == accessListId);
		}

		public async Task<List<AccessListEntity>> GetAll(Guid? clientId)
		{
			var queryable = GetReadOnlyQuery();
			var withClientQueryable = clientId.HasValue ? queryable.Where(oo => oo.ClientIdentifier == clientId.Value) : queryable;
			return await withClientQueryable.ToListAsync();
		}
	}
}
