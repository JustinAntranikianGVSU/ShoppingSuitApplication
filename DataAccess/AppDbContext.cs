using DataAccess.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<UserEntity> Users { get; set; }

		public DbSet<UserRoleEntity> UserRoles { get; set; }
	}
}
