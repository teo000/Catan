using Catan.Domain.Common;
using Catan.Domain.Data;
using Catan.Domain.Entities.GamePieces;
using Catan.Domain.Entities.Harbors;

namespace Catan.Domain.Entities.GameMap;

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

		var docksList = new List<int> { 0, 0, 0, 0, 1, 2, 3, 4, 5 };
		ListExtensions.Shuffle(docksList);

		for (int i = 0; i < docksList.Count; i++)
		{
			var dock = docksList[i];
			if (dock == 0)
				Harbors[i] = new Harbor(i);
			else 
				Harbors[i] = new SpecialHarbor(i, (Resource)dock);
		}
	}

	public HexTile[] HexTiles {  get; private set; } = new HexTile[GameMapData.HEX_TILE_NO];
	public int ThiefPosition { get; private set; }
	public Building[] Buildings { get; private set; } = new Building[GameMapData.SETTLEMENTS_NO];
	public Road[] Roads { get; private set; } = new Road[GameMapData.ROADS_NO];
	public Harbor[] Harbors { get; private set; } = new Harbor[GameMapData.HARBORS_NO];

	public void MoveThief(int position)
	{
		ThiefPosition = position;
	}

	


}
