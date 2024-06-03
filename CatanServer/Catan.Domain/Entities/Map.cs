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

			var numberTokenList = new List<int>(GameMapData.NumberTokenList);
			ListExtensions.Shuffle(numberTokenList);
			var index = 0;

			for (int i = 0; i < GameMapData.HEX_TILE_NO; i++) {
				var resource = resourceList[i];
				if (resource == Resource.Desert)
				{
					ThiefPosition = i;
					HexTiles[i] = new HexTile(resource, 0);
				}
				else
				{
					HexTiles[i] = new HexTile(resource, numberTokenList[index++]);
				}
			}

		}

		public HexTile[] HexTiles {  get; private set; } = new HexTile[GameMapData.HEX_TILE_NO];
		public int ThiefPosition { get; private set; }
		public GamePiece[] Buildings { get; private set; } = new GamePiece[GameMapData.SETTLEMENTS_NO];
		public Road[] Roads { get; private set; } = new Road[GameMapData.ROADS_NO];


		public void MoveThief(int position)
		{
			ThiefPosition = position;
		}

	}
}
