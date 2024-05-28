using Catan.Domain.Common;
using Catan.Domain.Data;

namespace Catan.Domain.Entities
{
	public class Map
	{
		public Map()
		{
			var valuesNo = Enum.GetValues(typeof(Resource)).Length;
			var random = new Random();

			var resourceList = GameMapData.getBeginningResourceList();
			ListExtensions.Shuffle(resourceList);

			for (int i = 0; i < GameMapData.HEX_TILE_NO; i++) {
				var resource = resourceList[i];
				HexTiles[i] = new HexTile(resource);
				if (resource == Resource.Desert)
					ThiefPosition = i;
			}

		}

		public HexTile[] HexTiles {  get; private set; } = new HexTile[GameMapData.HEX_TILE_NO];
		public int ThiefPosition { get; private set; }
		public Settlement[] Settlements { get; private set; } = new Settlement[GameMapData.SETTLEMENTS_NO];
		public Road[] Roads { get; private set; } = new Road[GameMapData.ROADS_NO];


		public void MoveThief(int position)
		{
			ThiefPosition = position;
		}

	}
}
