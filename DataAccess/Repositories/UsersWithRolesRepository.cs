using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
	public sealed class UsersWithRolesRepository : UserRepository
	{
		public UsersWithRolesRepository(AppDbContext dbContext) : base(dbContext) { }

		protected override IQueryable<UserEntity> GetReadOnlyQuery()
		{
			return _dbContext.Users
						.AsNoTracking()
						.Include(oo => oo.Roles);
		}
	}
}
