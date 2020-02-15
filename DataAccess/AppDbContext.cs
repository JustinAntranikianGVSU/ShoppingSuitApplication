using DataAccess.Entities;
using Domain.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataAccess
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<UserEntity> Users { get; set; }

		public DbSet<UserRoleEntity> UserRoles { get; set; }
	}

	public static class UserDbSetExtensions
	{
		/// <summary>
		/// Finds the first user that matches the email, if available.
		/// </summary>
		/// <param name="users"></param>
		/// <param name="email"></param>
		/// <returns></returns>
		public static async Task<UserEntity?> GetByEmail([CanBeNull] this DbSet<UserEntity> users, string email)
		{
			return await users.FirstOrDefaultAsync(oo => oo.Email == email);
		}
	}
}
