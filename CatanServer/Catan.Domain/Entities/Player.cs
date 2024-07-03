using Catan.Domain.Common;
using Catan.Domain.Data;
using Catan.Domain.Entities.GamePieces;
using Catan.Domain.Entities.Misc;
using Catan.Domain.Entities.Trades;
using System.Xml.Linq;

namespace Catan.Domain.Entities
{
    public class Player
	{
		public const int TOTAL_SETTLEMENTS_NO = 5;
		public static int TOTAL_CITIES_NO = 4;
		public Player(string name, bool isAI)
		{
			Id = Guid.NewGuid();
			Name = name;
			IsActive = true;

			ResourceCount = new Dictionary<Resource, int>();
			TradeCount = new Dictionary<Resource, int>();
			foreach (Resource resource in Enum.GetValues(typeof(Resource)))
				if (resource != Resource.Desert)
				{
					ResourceCount.Add(resource, 1);
					TradeCount.Add(resource, 0);
				}
			IsAI = isAI;
		}

		public Guid Id { get; private set; }
		public string Name { get; private set; }
		public bool IsActive { get; private set; }
		public Dictionary<Resource, int> ResourceCount { get; private set; }
		public Dictionary<Resource, int> TradeCount { get; private set; }
		public List<Settlement> Settlements { get; private set; } = new List<Settlement>();
		public List<City> Cities { get; private set; } = new List<City>();
		public List<Road> Roads { get; private set; } = new List<Road>();
		public List<DevelopmentCard> DevelopmentCards { get; private set; } = new List<DevelopmentCard>();
		public Color Color { get; set; }
		public int LastPlacedSettlementPos { get; set; } = -1;
		public int WinningPoints { get; set; }
		public bool IsAI { get; private set; }
		public bool DiscardedThisTurn { get; private set; } = false;
		public int KnightsPlayed { get; private set; }


		//ar trebui poate sa notez undeva ce fel de trade-uri din astea mai favorabile pot sa fac
		public static Result<Player> Create(string name) 
		{
			if (string.IsNullOrWhiteSpace(name))
				return Result<Player>.Failure("Player name cannot be empty.");
			return Result<Player>.Success(new Player(name, false));
		}

		public static Result<Player> CreateAI(int no)
		{
			return Result<Player>.Success(new Player($"Bot #{no}", true));
		}

		

		public void SetTradeCountSpecialPort(Resource resource)
		{
			TradeCount[resource] = 2;
		}

		public void SetTradeCountGeneralPort()
		{
			foreach (var (resource, count) in TradeCount) 
				if (count > 3)
					TradeCount[resource] = 3;
		}


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

		public bool HasResources(Dictionary<Resource, int> toSubtract)
		{
			foreach (var (resource, count) in toSubtract)
			{
				if (ResourceCount[resource] < count)
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

		public bool DiscardHalf (Dictionary<Resource, int> toSubtract)
		{
			foreach (var (resource, count) in toSubtract)
			{
				if (ResourceCount[resource] < count)
					return false;
			}

			foreach (var (resource, count) in toSubtract)
			{
				ResourceCount[resource] -= count;
			}

			DiscardedThisTurn = true;

			return true;
		}


		public bool HasResource(Resource resource, int count)
		{
			if (ResourceCount[resource] < count)
				return false;
			return true;
		}

		public void AssignResources (Dictionary<Resource, int> resourcesToAdd)
		{
			foreach (var (resource, count) in ResourceCount)
				if (resourcesToAdd.TryGetValue(resource, out var resourceCount))
				{
					ResourceCount[resource] += resourceCount;
				}
		}

		public void AssignResource(Resource resource, int count)
		{
			ResourceCount[resource] += count;
		}

		public Result<Player> SubtractResource (Resource resource, int count)
		{
			if (ResourceCount[resource] < count)
				return Result<Player>.Failure("You do not have enough resources.");

			ResourceCount[resource] -= count;
			return Result<Player>.Success(this);
		}

		public void Kick()
		{
			IsActive = false;
		}

		public int CalculatePoints()
		{
			return Settlements.Count + Cities.Count * 2;
		}

		public void AddSettlement(Settlement settlement)
		{
			Settlements.Add(settlement);
			LastPlacedSettlementPos = settlement.Position;
		}

		public void AddCity(City city)
		{
			Cities.Add(city);
			Settlements.RemoveAll(s => s.Position == city.Position);
		}

		public int GetCardsNo()
		{
			int cardsNo = 0;
			foreach (var (resource, count) in ResourceCount)
			{
				cardsNo += count;
			}
			return cardsNo;
		}

		public void SetDiscardedThisTurnFalse()
		{
			DiscardedThisTurn = false;
		}

		public void AddDevelopmentCard(DevelopmentCard newDevelopmentCard)
		{
			DevelopmentCards.Add(newDevelopmentCard);
		}

		public bool HasDevelopmentCard(DevelopmentType type)
		{
			foreach(var card in  DevelopmentCards)
			{
				if (card.DevelopmentType == type)
					return true;
			}

			return false;
		}

		public void PlayKnight()
		{
			var knightCard = DevelopmentCards.Where(c => c.DevelopmentType == DevelopmentType.KNIGHT).FirstOrDefault();
			DevelopmentCards.Remove(knightCard);
			KnightsPlayed++;
		}
	}
}
