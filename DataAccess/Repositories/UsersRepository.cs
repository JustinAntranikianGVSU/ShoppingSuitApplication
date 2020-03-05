using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
	}
}
