using Catan.Domain.Data;
using System.Text.Json.Serialization;

namespace Catan.Domain.Entities
{
	public class HexTile : Tile
	{
		public HexTile(Resources resource)
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
		public Resources Resource { get; private set; }
		public int Number {  get; private set; }
		public List<Settlement> Settlements { get; private set; }
	}
}
