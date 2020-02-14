using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
	public class OccupationSector
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }
	}

	public class Occupation
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int OccupationSectorId { get; set; }
	}
}
