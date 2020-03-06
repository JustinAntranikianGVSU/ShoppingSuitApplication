﻿using DataAccess.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
	public class LocationsRepository : BaseRepository
	{
		public LocationsRepository(AppDbContext dbContext) : base(dbContext) {}

		public IQueryable<LocationEntity> GetLocationsWithAccessListsQuery()
		{
			return _dbContext.Locations
						.AsNoTracking()
						.Include(oo => oo.AccessLists).ThenInclude(oo => oo.AccessList);
		}

		public IQueryable<UserEntity> GetUsers(int locationId)
		{
			return _dbContext.AccessListLocations
						.AsNoTracking()
						.Where(oo => oo.LocationId == locationId)
						.SelectMany(oo => oo.AccessList.Users)
						.Select(oo => oo.User);
		}
	}
}
