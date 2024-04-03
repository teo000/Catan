using Microsoft.AspNetCore.Mvc;

namespace Catan.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GameController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<GameController> _logger;

		public GameController(ILogger<GameController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public List<HexTile> Get()
		{
			var map = new Map();
			return map.tiles;
		}
	}
}
