using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table("Location")]
	public class LocationEntity
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

		public Guid ClientIdentifier { get; set; }

		[ForeignKey("LocationId")]
		public ICollection<AccessListLocationEntity> AccessLists { get; set; } = new List<AccessListLocationEntity>();
	}
}
