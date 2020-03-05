using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table("AccessList_Location")]
	public class AccessListLocationEntity
	{
		[Key]
		public int Id { get; set; }

		public int AccessListId { get; set; }

		public AccessListEntity AccessList { get; set; }

		public int LocationId { get; set; }

		public LocationEntity Location { get; set; }
	}
}
