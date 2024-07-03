using AIService.Entities.Data;
using AIService.Entities.Game.GamePieces;
using AIService.Entities.Game.Harbors;
using Catan.Data;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

		public List<int> GetViableSettlementPositions(Player player)
		{
			var settlementPositions = Settlements.Select(s => s.Position).ToList();
			var citiesPostions = Cities.Select(c => c.Position).ToList();
			var buildingsPositions = settlementPositions.Concat(citiesPostions).ToList();

			var playerRoadPositions = Roads
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


		public bool IsPositionViableForSettlement(int settlement)
		{
			var settlementPositions = Settlements.Select(s => s.Position).ToList();
			var citiesPostions = Cities.Select(c => c.Position).ToList();
			var buildingsPositions = settlementPositions.Concat(citiesPostions).ToList();

			if (buildingsPositions.Contains(settlement))
				return false;


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

			return ok;

		}

		public bool SettlementAtPosition(int settlement)
		{
			var settlementPositions = Settlements.Select(s => s.Position).ToList();
			var citiesPostions = Cities.Select(c => c.Position).ToList();
			var buildingsPositions = settlementPositions.Concat(citiesPostions).ToList();

			if (buildingsPositions.Contains(settlement))
				return true;
			return false;
		}

		public bool RoadAtPosition(int road)
		{
			var roadPositions = Roads.Select(r => r.Position).ToList();

			if (roadPositions.Contains(road))
				return true;

			return false;

		}

		private static List<int> GetAdjacentPositions(int position)
		{
			return GameMapData.AdjacentSettlements[position];
		}

		public List<int> GetViableBeginningSettlementPositions()
		{
			var viablePositions = new List<int>();

			for (int position = 0; position < GameMapData.SETTLEMENTS_NO; position++)
			{
				if (IsPositionViableForSettlement(position))
				{
					viablePositions.Add(position);
				}
			}

			return viablePositions;
		}

		public List<int> GetViableRoadPositions(Player player)
		{
			var viableRoadPositions = new List<int>();

			var roadPositions = Roads.Select(r => r.Position).ToList();

			var playerRoads = Roads
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

		public List<RoadToSettlement> GetViableRoadsToSettlement(Player player)
		{
			var viableSpots = new List<RoadToSettlement>();
			var checkedPositions = new HashSet<int>();

			var roadPositions = Roads.Select(r => r.Position).ToList();

			foreach (var settlement in player.Settlements)
			{
				var adjacentPositions = GetAdjacentPositions(settlement.Position);
				foreach (var adjPosition in adjacentPositions)
				{
					var secondLevelPositions = GetAdjacentPositions(adjPosition);
					foreach (var secondLevelPosition in secondLevelPositions)
					{
						if (!checkedPositions.Contains(secondLevelPosition) &&
							!SettlementAtPosition(secondLevelPosition))
						{
							var road1 = Road.GetRoadPosition(settlement.Position, adjPosition);
							var road2 = Road.GetRoadPosition(adjPosition, secondLevelPosition);
							if (road1 != -1 && road2 != -1 && !roadPositions.Contains(road2))
							{
								var roads = new List<int> { road1, road2 };
								viableSpots.Add(new RoadToSettlement(secondLevelPosition, roads));
								checkedPositions.Add(secondLevelPosition);
							}
						}
					}
				}
			}

			return viableSpots;
		}

		public int GetSettlementResourceValue(int position)
		{
			int value = 0;
			foreach (int settlementPos in GameMapData.SettlementAdjacentHexes[position])
				value += HexTiles[settlementPos].Value();
			return value;
		}

		public int GetRoadTo(int bestSettlementPos)
		{
			foreach(var settlement in Settlements)
			{
				var roadPosition = Road.GetRoadPosition(settlement.Position, bestSettlementPos);

				if (roadPosition != -1)
					return roadPosition;
			}

			return -1;
		}

	}
}