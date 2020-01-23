using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ShoppingSuitePlatform.Controllers
{
	public class Entry
	{
		public Entry() { }

		public Entry(string entry) => EntryName = entry;

		public string? EntryName { get; set; }
	}

	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			var entry = new Entry
			{
				EntryName = "AAAAA"
			};

			var entries = new Entry[]
			{
				new Entry("as0"),
				new Entry("as1"),
				new Entry("as2"),
				new Entry("as3"),
				new Entry("as4"),
			};

			var more11 = entries[1..2];
			var more112 = entries[0..2];
			var more1112 = entries[^2..];
			var more1111 = entries[^1..];
			var more1110 = entries[^0];

			var rng = new Random();
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			})
			.ToArray();
		}
	}
}
