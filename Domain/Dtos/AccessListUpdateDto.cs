using System.Collections.Generic;

namespace Domain.Dtos
{
	public class AccessListUpdateDto
	{
		public string Name { get; set; }

		public List<int> LocationIds { get; set; }

		public List<int> UserIds { get; set; }
	}
}
