using AIService.Entities.Data;
using System.Collections.ObjectModel;

namespace Catan.Data;

public static class GameInfo
{
	public const int TURN_DURATION = 3000;
	public const int SETTLEMENTS_PER_PLAYER = 5;
	public const int CITIES_PER_PLAYER = 4;
	public const int WINNING_POINTS = 10;
	public static ReadOnlyDictionary<Buyable, Dictionary<Resource, int>> Costs;

	static GameInfo() 
	{
		Dictionary<Buyable, Dictionary<Resource, int>> costs = new Dictionary<Buyable, Dictionary<Resource, int>>()
		{
			{
				Buyable.SETTLEMENT, new Dictionary<Resource, int>()
				{
					{ Resource.Brick, 1 },
					{ Resource.Wheat, 1 },
					{ Resource.Sheep, 1 },
					{ Resource.Wood, 1 },
				}
			},
			{
				Buyable.CITY, new Dictionary<Resource, int>()
				{
					{ Resource.Wheat, 2 },
					{ Resource.Ore, 3 },
				}
			},
			{
				Buyable.ROAD, new Dictionary<Resource, int>()
				{
					{ Resource.Brick, 1 },
					{ Resource.Wood, 1 },
				}
			},
			{
				Buyable.DEVELOPMENT, new Dictionary<Resource, int>()
				{
					{ Resource.Sheep,1 },
					{ Resource.Wheat, 1 },
					{ Resource.Ore, 1 },
				}
			}
		};

		Costs = new ReadOnlyDictionary<Buyable, Dictionary<Resource, int>>(costs);
	}


}
