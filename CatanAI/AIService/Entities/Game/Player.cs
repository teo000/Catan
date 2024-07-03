using AIService.Entities.Data;
using AIService.Entities.Game.GamePieces;
using Catan.Data;
using Catan.Entities.Data;

namespace AIService.Entities.Game
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Dictionary<Resource, int> ResourceCount { get; set; }
        public Dictionary<Resource, int> TradeCount { get; set; }
        public List<Settlement> Settlements { get; set; }
        public List<City> Cities { get; set; }
        public List<Road> Roads { get; set; }
        public Color Color { get; set; }
        public int LastPlacedSettlementPos { get; set; }
        public int WinningPoints { get; set; }
        public bool IsAI { get; set; }

		public bool HasResources(Buyable buyable)
		{
			var costs = GameInfo.Costs[buyable];
			foreach (var (resource, required) in costs)
			{
				if (ResourceCount[resource] < required)
					return false;
			}
			return true;
		}
		public bool SubtractResources(Buyable buyable)
		{
			var costs = GameInfo.Costs[buyable];
			foreach (var (resource, required) in costs)
			{
				if (ResourceCount[resource] < required)
					return false;
			}

			foreach (var (resource, required) in costs)
			{
				ResourceCount[resource] -= required;
			}

			return true;
		}

		public int GetCardsNo()
		{
			int no = 0;
			foreach (var (resource, count) in ResourceCount)
			{
				no += count;
			}
			return no;
		}

		public Dictionary<Resource, int> CardsNeeded(Buyable buyable)
		{
			var costs = GameInfo.Costs[buyable];
			var cardsNeeded = new Dictionary<Resource, int>();
			foreach (var (resource, required) in costs)
			{
				int differenceNeeded = required - ResourceCount[resource];
				if (differenceNeeded > 0)
					cardsNeeded.Add(resource, differenceNeeded);
			}

			return cardsNeeded;
		}

		public Resource? GetMostAbundantResource()
		{
			var abundantResource = ResourceCount
			.Where(rc => rc.Value > 0)
			.OrderByDescending(rc => rc.Value)
			.FirstOrDefault();

			if (abundantResource.Key == default)
				return null; 

			return abundantResource.Key;
		}
	}
}