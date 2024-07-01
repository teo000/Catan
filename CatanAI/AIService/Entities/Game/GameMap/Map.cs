using AIService.Entities.Game.GamePieces;
using AIService.Entities.Game.Harbors;
using Catan.Data;

namespace AIService.Entities.Game.GameMap
{
    public class Map
    {
        public HexTile[] HexTiles { get; set; }
        public int ThiefPosition { get; set; }
        public List<Settlement> Settlements { get; set; }
        public List<City> Cities { get; set; }
        public List<Road> Roads { get; set; }
        public SpecialHarbor[] SpecialHarbors { get; set; }

		public static List<int> GetViableSettlementPositions(GameState gameState, Player player)
		{
			var settlementPositions = gameState.Map.Settlements.Select(s => s.Position).ToList();
			var citiesPostions = gameState.Map.Cities.Select(c => c.Position).ToList();
			var buildingsPositions = settlementPositions.Concat(citiesPostions).ToList();

			var playerRoadPositions = gameState.Map.Roads
				.Where(s => s.PlayerId == player?.Id)
				.Select(s => s.Position)
				.ToList();

			var possibleSettlementPositions = new List<int>();

			foreach (var road in playerRoadPositions)
			{
				var ends = GameMapData.RoadEnds[road];
				var endPositions = new List<int> { ends.Item1, ends.Item2 };

				foreach (var settlement in endPositions)
				{
					if (!buildingsPositions.Contains(settlement))
					{
						var ok = true;
						if (GameMapData.AdjacentSettlements.TryGetValue(settlement, out var adjacents))
						{
							foreach (var adjacent in adjacents)
							{
								if (buildingsPositions.Contains(adjacent))
								{
									ok = false;
									break;
								}
							}
						}
						if (ok)
						{
							possibleSettlementPositions.Add(settlement);
						}
					}
				}
				
			}

			return possibleSettlementPositions;
		}

		public static List<int> GetViableRoadPositions(GameState gameState, Player player)
		{
			var viableRoadPositions = new List<int>();

			var roadPositions = gameState.Map.Roads.Select(r => r.Position).ToList();

			var playerRoads = gameState.Map.Roads
				.Where(r => r.PlayerId == player?.Id)
				.Select(r => r.Position)
				.ToList();

			foreach (var road in playerRoads)
			{
				var ends = GameMapData.RoadEnds[road];
				var endPositions = new List<int> { ends.Item1, ends.Item2 };

				foreach (var end in endPositions)
				{
					if (GameMapData.AdjacentSettlements.TryGetValue(end, out var settlements))
					{
						foreach (var settlement in settlements)
						{
						
							var roadPosition = Road.GetRoadPosition(end, settlement);

							if (roadPosition != -1 && !roadPositions.Contains(roadPosition))
							{
								viableRoadPositions.Add(roadPosition);
							}
						}
					}
				}
			}
			return viableRoadPositions;
		}

	}
}