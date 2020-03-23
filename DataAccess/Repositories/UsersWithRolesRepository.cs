﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public sealed class UsersWithRolesRepository : UserBaseRepository
	{
		public UsersWithRolesRepository(AppDbContext dbContext) : base(dbContext) { }

		public override IQueryable<UserEntity> GetReadOnlyQuery()
		{
			return _dbContext.Users
						.AsNoTracking()
						.Include(oo => oo.Roles);
		}

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
