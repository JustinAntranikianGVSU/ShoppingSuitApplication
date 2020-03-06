﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class UsersWithRolesRepository : BaseRepository
	{
		public UsersWithRolesRepository(AppDbContext dbContext) : base(dbContext) {}

		public IQueryable<UserEntity> GetReadOnlyQuery() => _dbContext.Users.Include(oo => oo.Roles).AsNoTracking();

		public async Task<UserEntity> SingleAsync(int userId)
		{
			return await GetReadOnlyQuery().SingleAsync(oo => oo.Id == userId);
		}

		public async Task<UserEntity?> SingleOrDefaultAsync(int userId)
		{
			return await GetReadOnlyQuery().SingleOrDefaultAsync(oo => oo.Id == userId);
		}
	}
}
