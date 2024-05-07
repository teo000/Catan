using CatanGame.Data;
using System.Text.Json.Serialization;

namespace CatanGame.Entities
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

			Settlements = new List<Settlement>();
		}

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Resource Resource { get; private set; }
		public int Number {  get; private set; }
		public List<Settlement> Settlements { get; private set; }
	}
}
