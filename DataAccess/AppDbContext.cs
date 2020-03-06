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
}
