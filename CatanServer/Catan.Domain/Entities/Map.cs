using Catan.Domain.Data;

namespace Catan.Domain.Entities
{
	public class Map
	{
		public Map()
		{
			var valuesNo = Enum.GetValues(typeof(Resource)).Length;
			var random = new Random();

			HexTiles = Enumerable.Range(0, 19)
						 .Select(_ => new HexTile((Resource)random.Next(1, valuesNo)))
						 .ToArray();

			int desertIndex = random.Next(0, 19);
			HexTiles[desertIndex] = new HexTile(Resource.Desert);
			ThiefPosition = desertIndex;
		}

		public HexTile[] HexTiles {  get; private set; }
		public int ThiefPosition { get; private set; }
		public Settlement[] Settlements { get; private set; } = new Settlement[GameMapData.SETTLEMENTS_NO];
		public Road[] Roads { get; private set; } = new Road[GameMapData.ROADS_NO];


		public void MoveThief(int position)
		{
			ThiefPosition = position;
		}

	}
}
