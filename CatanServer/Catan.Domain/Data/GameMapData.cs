using Catan.Domain.Entities;

namespace Catan.Domain.Data
{
	public static class GameMapData
	{
		public const int HEX_TILE_NO = 19;
		public const int ROWS_NO = 5;
		public const int SETTLEMENTS_NO = 54;
		public const int ROADS_NO = 72;

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

				List<int> lastAdjacentTiles = new List<int>();

				lastAdjacentTiles.Add(HexTileRowLayout[row][HexTileRowLayout[row].Count - 1]);
				SettlementAdjacentTiles.Add(lastAdjacentTiles);


				for (int pos = 0; pos < HexTileRowLayout[row].Count; pos++)
				{
					List<int> adjacentTiles = new List<int>();


					adjacentTiles.Add(HexTileRowLayout[row][pos]);

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


			for(int row=0; row < SettlementLayout.Count - 1; row++)
			{
				if (SettlementLayout[row].Count < SettlementLayout[row + 1].Count)
				{
					for(int pos = 0; pos < SettlementLayout[row].Count; pos++)
					{
						RoadEnds.Add((SettlementLayout[row + 1][pos], SettlementLayout[row][pos]));
						RoadEnds.Add((SettlementLayout[row][pos], SettlementLayout[row + 1][pos + 1]));
					}
				}
				else if (SettlementLayout[row].Count == SettlementLayout[row + 1].Count)
				{
					for (int pos = 0; pos < SettlementLayout[row].Count; pos++)
					{
						RoadEnds.Add((SettlementLayout[row][pos], SettlementLayout[row + 1][pos]));
					}
				}
				else
				{
					for (int pos = 0; pos < SettlementLayout[row+1].Count; pos++)
					{
						RoadEnds.Add((SettlementLayout[row][pos], SettlementLayout[row + 1][pos]));
						RoadEnds.Add((SettlementLayout[row + 1][pos], SettlementLayout[row][pos + 1]));
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
		}

		public static List<List<int>> SettlementAdjacentTiles = new List<List<int>>();

		public static List<(int, int)> RoadEnds = new List<(int, int)>();

		public static Dictionary<int, List<int>> AdjacentSettlements = new Dictionary<int, List<int>>();

		//public static Dictionary<int, List<int>> SettlementAdjacentTiles = new Dictionary<int, List<int>>()
		//{
		//	{0, new List<int> {0}},
		//	{1, new List<int> {1}},
		//	{2, new List<int> {2}},
		//	{3, new List<int> {0}},

		//	{4, new List<int> {0}},
		//	{5, new List<int> {0, 1}},
		//	{6, new List<int> {1, 2}},
		//	{7, new List<int> {2 }},

		//	{8, new List<int> {0, 3}},
		//	{9, new List<int> {0, 1, 4}},
		//	{10, new List<int> {1, 2, 5 } },
		//	{11, new List<int> {2, 6 } },

		//	{12, new List<int> {3 } },
		//	{13, new List<int> {0, 3, 4 } },
		//	{14, new List<int> {1, 4, 5 } },
		//	{15, new List<int> {2, 5, 6 } },
		//	{16, new List<int> {2, 6}},

		//	{17, new List<int> {3, 7 } },
		//	{18, new List<int> {3, 4, 8} },
		//	{19, new List<int> {4, 5, 9 } },
		//	{20, new List<int> {6, 7, 11} },
		//	{21, new List<int> {7, 12} },

		//	{22, new List<int> {8 } },
		//	{23, new List<int> {4, 8, 9} },
		//	{24, new List<int> {} }


		//};


	}
}
