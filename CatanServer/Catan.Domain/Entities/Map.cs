using Catan.Domain.Data;

namespace Catan.Domain.Entities
{
	public class Map
	{
		public Map()
		{
			var valuesNo = Enum.GetValues(typeof(Resource)).Length;
			var random = new Random();

			hexTiles = Enumerable.Range(0, 19)
						 .Select(_ => new HexTile((Resource)random.Next(1, valuesNo)))
						 .ToList();

			int desertIndex = random.Next(0, 19);
			hexTiles[desertIndex] = new HexTile(Resource.Desert);
			ThiefPosition = desertIndex;
		}

		public List<HexTile> hexTiles {  get; private set; }
		public int ThiefPosition { get; private set; }

	}
}
