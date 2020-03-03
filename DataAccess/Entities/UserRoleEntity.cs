using Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table("User_Role")]
	public class UserRoleEntity
	{
		[Key]
		public int Id { get; set; }

		public int UserId { get; set; }

		public Guid RoleGuid { get; set; }

		public UserEntity User { get; set; }
	}
}
