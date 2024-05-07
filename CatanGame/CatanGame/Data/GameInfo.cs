using System.Collections.ObjectModel;

namespace CatanGame.Data
{
	public static class GameInfo
	{
		public static ReadOnlyDictionary<Buyables, Dictionary<Resources, int>> Costs;

		static GameInfo() 
		{
			Dictionary<Buyables, Dictionary<Resources, int>> costs = new Dictionary<Buyables, Dictionary<Resources, int>>()
			{
				{
					Buyables.SETTLEMENT, new Dictionary<Resources, int>()
					{
						{ Resources.Brick, 1 },
						{ Resources.Wheat, 1 },
						{ Resources.Sheep, 1 },
						{ Resources.Wood, 1 },
					}
				},
				{
					Buyables.CITY, new Dictionary<Resources, int>()
					{
						{ Resources.Wheat, 2 },
						{ Resources.Ore, 3 },
					}
				},
				{
					Buyables.ROAD, new Dictionary<Resources, int>()
					{
						{ Resources.Brick, 1 },
						{ Resources.Wood, 1 },
					}
				},
				{
					Buyables.DEVELOPMENT, new Dictionary<Resources, int>()
					{
						{ Resources.Sheep,1 },
						{ Resources.Wheat, 1 },
						{ Resources.Ore, 1 },
					}
				}
			};

			Costs = new ReadOnlyDictionary<Buyables, Dictionary<Resources, int>>(costs);
		}


	}
}
