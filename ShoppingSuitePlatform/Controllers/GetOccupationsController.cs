using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GetOccupationsController : ControllerBase
	{
		[HttpGet]
		public async Task<List<Occupation>> Get()
		{
			var occupations = new List<Occupation>
			{
				new Occupation { Id = 1, Name = "Software Developer", OccupationSectorId = 1 },
				new Occupation { Id = 2, Name = "Dev Ops", OccupationSectorId = 1 },
				new Occupation { Id = 3, Name = "Nurse", OccupationSectorId = 2 },
				new Occupation { Id = 4, Name = "Medical Billing", OccupationSectorId = 2 },
				new Occupation { Id = 5, Name = "Doctor", OccupationSectorId = 2 },
				new Occupation { Id = 6, Name = "Police", OccupationSectorId = 4 },
				new Occupation { Id = 7, Name = "Navy", OccupationSectorId = 4 },
			};

			return occupations;
		}
	}
}
