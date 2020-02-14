using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OccupationSectorsController : ControllerBase
	{
		[HttpGet]
		public async Task<List<OccupationSector>> Get()
		{
			var occupations = new List<OccupationSector>
			{
				new OccupationSector { Id = 1, Name = "IT" },
				new OccupationSector { Id = 2, Name = "Medical" },
				new OccupationSector { Id = 3, Name = "Social Services" },
				new OccupationSector { Id = 4, Name = "Military/Police" }
			};

			return occupations;
		}
	}
}
