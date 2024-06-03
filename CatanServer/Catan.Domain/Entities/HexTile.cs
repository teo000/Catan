using Catan.Domain.Data;
using System.Text.Json.Serialization;

namespace Catan.Domain.Entities
{
	public class HexTile : Tile
	{
		public HexTile(Resource resource, int number)
		{
			Resource = resource;
			Number = number;
		}

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Resource Resource { get; private set; }
		public int Number {  get; private set; }
		public List<Building> Buildings { get; private set; } = new List<Building>();
	}
}
