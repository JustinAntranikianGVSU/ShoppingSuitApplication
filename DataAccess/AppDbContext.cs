using DataAccess.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataAccess
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<UserEntity> Users { get; set; }

		public DbSet<UserRoleEntity> UserRoles { get; set; }

		public DbSet<UserAccessListEntity> UserAccessLists { get; set; }

		public DbSet<LocationEntity> Locations { get; set; }

		public DbSet<AccessListEntity> AccessLists { get; set; }

		public DbSet<AccessListLocationEntity> AccessListLocations { get; set; }
	}

	public static class UserDbSetExtensions
	{
		/// <summary>
		/// Finds the first user that matches the email, if available.
		/// </summary>
		/// <param name="users"></param>
		/// <param name="email"></param>
		/// <returns></returns>
		public static async Task<UserEntity?> GetByEmail(this DbSet<UserEntity> users, string email)
		{
			return await users.FirstOrDefaultAsync(oo => oo.Email == email);
		}

		public static async Task<UserEntity?> GetById(this DbSet<UserEntity> users, int id) => await users.SingleOrDefaultAsync(oo => oo.Id == id);
	}
}
