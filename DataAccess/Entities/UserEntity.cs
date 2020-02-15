
using DataAccess.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	[Table("Users")]
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

		[ForeignKey("UserId")]
		public virtual ICollection<UserRoleEntity> Roles { get; set; } = new List<UserRoleEntity>();
	}
}
