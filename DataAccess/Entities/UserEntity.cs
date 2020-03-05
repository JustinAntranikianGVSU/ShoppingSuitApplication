
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	[Table("User")]
	public class UserEntity
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(50)]
		public string LastName { get; set; }

		[Required]
		[MaxLength(50)]
		public string Email { get; set; }

		public Guid? ClientIdentifier { get; set; }

		[ForeignKey("UserId")]
		public ICollection<UserRoleEntity> Roles { get; set; } = new List<UserRoleEntity>();

		[ForeignKey("UserId")]
		public ICollection<UserAccessListEntity> UserAccessListEntities { get; set; } = new List<UserAccessListEntity>();
	}
}
