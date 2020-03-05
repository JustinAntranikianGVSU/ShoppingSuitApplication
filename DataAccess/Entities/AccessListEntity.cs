using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table("AccessList")]
	public class AccessListEntity
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

		public Guid ClientIdentifier { get; set; }

		[ForeignKey("AccessListId")]
		public ICollection<AccessListLocationEntity> Locations { get; set; } = new List<AccessListLocationEntity>();

		[ForeignKey("AccessListId")]
		public ICollection<UserAccessListEntity> Users { get; set; } = new List<UserAccessListEntity>();
	}
}
