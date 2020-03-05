using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table("User_AccessList")]
	public class UserAccessListEntity
	{
		[Key]
		public int Id { get; set; }

		public int AccessListId { get; set; }

		public AccessListEntity AccessList { get; set; }

		public int UserId { get; set; }

		public UserEntity User { get; set; }
	}
}
