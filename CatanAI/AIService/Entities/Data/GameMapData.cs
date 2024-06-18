using AIService.Entities.Data;

namespace Catan.Data;

public static class GameMapData
{
	public const int HEX_TILE_NO = 19;
	public const int ROWS_NO = 5;
	public const int SETTLEMENTS_NO = 54;
	public const int ROADS_NO = 72;
	public const int HARBORS_NO = 9;

	public static Dictionary<int, List<int>> HexTileRowLayout = new Dictionary<int, List<int>>() {
		{0, new List<int>() {      0,  1,  2      } },
		{1, new List<int>() {    3,  4,  5,  6    } },
		{2, new List<int>() {  7,  8,  9, 10, 11  } },
		{3, new List<int>() {   12, 13, 14, 15    } },
		{4, new List<int>() {      16, 17, 18      } }
	};

	public static Dictionary<int, List<int>> SettlementLayout = new Dictionary<int, List<int>>()
	{
		{0, new List<int> { 0, 1, 2,} },
		{1, new List<int> { 3, 4, 5, 6 } },
		{2, new List<int> { 7, 8, 9, 10 } },
		{3, new List<int> { 11, 12, 13, 14, 15 } },
		{4, new List<int> { 16, 17, 18, 19, 20 } },
		{5, new List<int> { 21, 22, 23, 24, 25, 26 } },
		{6, new List<int> { 27, 28, 29, 30, 31, 32 } },
		{7, new List<int> { 33, 34, 35, 36, 37 } },
		{8, new List<int> { 38, 39, 40, 41, 42 } },
		{9, new List<int> { 43, 44, 45, 46}},
		{10, new List<int> { 47, 48, 49, 50}},
		{11, new List<int> { 51, 52, 53} }
	};

	static GameMapData()
	{
		for (int row = 0; row <= ROWS_NO / 2; row++)
		{
			for (int pos = 0; pos < HexTileRowLayout[row].Count; pos++)
			{
				List<int> adjacentTiles = new List<int>();

				if (row >= 1)
				{
					if (pos >= 1)
						adjacentTiles.Add(HexTileRowLayout[row - 1][pos - 1]);

					if (pos < HexTileRowLayout[row - 1].Count)
						adjacentTiles.Add(HexTileRowLayout[row - 1][pos]);
				}

				adjacentTiles.Add(HexTileRowLayout[row][pos]);

				SettlementAdjacentTiles.Add(adjacentTiles);
			}

			for (int pos = 0; pos < HexTileRowLayout[row].Count; pos++)
			{
				List<int> adjacentTiles = new List<int>();


				if (pos >= 1)
				{
					if (row >= 1)
					{
						adjacentTiles.Add(HexTileRowLayout[row - 1][pos - 1]);
					}

					adjacentTiles.Add(HexTileRowLayout[row][pos - 1]);
				}

				if (pos < HexTileRowLayout[row].Count)
					adjacentTiles.Add(HexTileRowLayout[row][pos]);

				SettlementAdjacentTiles.Add(adjacentTiles);
			}

			List<int> lastAdjacentTiles = new List<int>();
			if (row >= 1)
				lastAdjacentTiles.Add(HexTileRowLayout[row - 1][HexTileRowLayout[row - 1].Count - 1]);
			lastAdjacentTiles.Add(HexTileRowLayout[row][HexTileRowLayout[row].Count - 1]);
			SettlementAdjacentTiles.Add(lastAdjacentTiles);
		}

		for (int row = ROWS_NO/2; row < ROWS_NO; row++)
		{
			for (int pos = 0; pos < HexTileRowLayout[row].Count; pos++)
			{
				List<int> adjacentTiles = new List<int>();


				if (pos >= 1)
					adjacentTiles.Add(HexTileRowLayout[row][pos - 1]);

				adjacentTiles.Add(HexTileRowLayout[row][pos]);
			
				
				
				if(row < ROWS_NO - 1 && pos >= 1)
					adjacentTiles.Add(HexTileRowLayout[row+1][pos-1]);

				SettlementAdjacentTiles.Add(adjacentTiles);
			}

			List<int> lastAdjacentTiles = [HexTileRowLayout[row][HexTileRowLayout[row].Count - 1]];
			SettlementAdjacentTiles.Add(lastAdjacentTiles);


			for (int pos = 0; pos < HexTileRowLayout[row].Count; pos++)
			{
				List<int> adjacentTiles = [HexTileRowLayout[row][pos]];

				if (row < ROWS_NO - 1)
				{
					if(pos >= 1)
						adjacentTiles.Add(HexTileRowLayout[row + 1][pos - 1]);

					if(pos < HexTileRowLayout[row + 1].Count)
					adjacentTiles.Add(HexTileRowLayout[row + 1][pos]);
				}

				SettlementAdjacentTiles.Add(adjacentTiles);
			}
		}

		int index = 0;
		for(int row=0; row < SettlementLayout.Count - 1; row++)
		{
			if (SettlementLayout[row].Count < SettlementLayout[row + 1].Count)
			{
				for(int pos = 0; pos < SettlementLayout[row].Count; pos++)
				{
					var roadEnd1 = (SettlementLayout[row + 1][pos], SettlementLayout[row][pos]);
					var roadEnd2 = (SettlementLayout[row][pos], SettlementLayout[row + 1][pos + 1]);

					RoadEnds.Add(roadEnd1);
					RoadByRoadEnds.Add(roadEnd1, index++);

					RoadEnds.Add(roadEnd2);
					RoadByRoadEnds.Add(roadEnd2, index++);
				
				}
			}
			else if (SettlementLayout[row].Count == SettlementLayout[row + 1].Count)
			{
				for (int pos = 0; pos < SettlementLayout[row].Count; pos++)
				{
					var roadEnd = (SettlementLayout[row][pos], SettlementLayout[row + 1][pos]);
					RoadEnds.Add(roadEnd);
					RoadByRoadEnds.Add(roadEnd, index++);
				}
			}
			else
			{
				for (int pos = 0; pos < SettlementLayout[row+1].Count; pos++)
				{
					var roadEnd1 = (SettlementLayout[row][pos], SettlementLayout[row + 1][pos]);
					var roadEnd2 = (SettlementLayout[row + 1][pos], SettlementLayout[row][pos + 1]);

					RoadEnds.Add(roadEnd1);
					RoadByRoadEnds.Add(roadEnd1, index++);

					RoadEnds.Add(roadEnd2);
					RoadByRoadEnds.Add(roadEnd2 , index++);
				}
			}
		}


		foreach((int, int) road in RoadEnds)
		{
			int settlement1 = road.Item1;
			int settlement2 = road.Item2;

			if (AdjacentSettlements.ContainsKey(settlement1))
				AdjacentSettlements[settlement1].Add(settlement2);
			else 
				AdjacentSettlements.Add(settlement1, new List<int> () { settlement2 });

			if (AdjacentSettlements.ContainsKey(settlement2))
				AdjacentSettlements[settlement2].Add(settlement1);
			else
				AdjacentSettlements.Add(settlement2, new List<int>() { settlement1 });
		}

		for (var harbor = 0; harbor < SettlementsNextToHarbor.Count; harbor ++)
		{
			var (settlement1, settlement2) = SettlementsNextToHarbor[harbor];
			HarbourNextToSettlement.Add(settlement1, harbor);
			HarbourNextToSettlement.Add(settlement2, harbor);
		}

	}

	public static List<List<int>> SettlementAdjacentTiles = new List<List<int>>();

	public static List<(int, int)> RoadEnds = new List<(int, int)>();

	public static Dictionary<(int, int), int> RoadByRoadEnds = new Dictionary<(int, int), int>();

	public static Dictionary<int, List<int>> AdjacentSettlements = new Dictionary<int, List<int>>();
	public static readonly List<int> NumberTokenList 
		= [2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12];

	public static List<(int, int)> SettlementsNextToHarbor =
	[
		(0, 3),
		(1, 5),
		(10, 15),
		(26, 32),
		(42, 46),
		(49, 52),
		(47, 51),
		(33, 38),
		(11, 16),
	];

	public static Dictionary<int, int> HarbourNextToSettlement = new Dictionary<int, int>();

	public static List<Resource> getBeginningResourceList()
	{
		List<Resource> resources = new List<Resource>();

		for (int i = 0; i < 4; i++)
		{
			resources.Add(Resource.Sheep);
			resources.Add(Resource.Wheat);
			resources.Add(Resource.Wood);

		}

		for (int i = 0; i < 3; i++)
		{
			resources.Add(Resource.Ore);
			resources.Add(Resource.Brick);
		}

            resources.Add(Resource.Desert);
		return resources;
	}

}
