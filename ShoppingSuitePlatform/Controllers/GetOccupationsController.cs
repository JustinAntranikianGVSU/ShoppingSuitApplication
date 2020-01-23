using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	public class GetOccupationsController : ControllerBase
	{
		[HttpGet]
		public async Task<List<Occupation>> Get()
		{
			var occupations = new List<Occupation>();
			return occupations;
		}
	}
}
