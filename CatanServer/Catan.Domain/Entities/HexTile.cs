using Catan.Domain.Data;
using System.Text.Json.Serialization;

namespace Catan.Domain.Entities
{
	public class HexTile : Tile
	{
		public HexTile(Resource resource)
		{
			Resource = resource;

			var random = new Random();
			do
			{
				Number = random.Next(2, 13);
			} while (Number == 7);
		}

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Resource Resource { get; private set; }
		public int Number {  get; private set; }
		public List<Building> Buildings { get; private set; } = new List<Building>();
	}
}
