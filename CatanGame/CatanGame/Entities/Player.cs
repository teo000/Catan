using CatanGame.Common;
using CatanGame.Data;

namespace CatanGame.Entities
{
	public class Player
	{
		public const int TOTAL_SETTLEMENTS_NO = 5;
		public static int TOTAL_CITIES_NO = 4;
		public Player(string name)
		{
			Id = Guid.NewGuid();
			Name = name;
			IsActive = true;
			ResourceCount = new Dictionary<Resources, int>();
			foreach (Resources resource in Enum.GetValues(typeof(Resources)))
			{
				ResourceCount.Add(resource, 0);
			}
		}

		public Guid Id { get; private set; }
		public string Name { get; private set; }
		public bool IsActive { get; private set; }
		public Dictionary<Resources, int> ResourceCount { get; private set; }
		public List<Settlement> Settlements { get; private set; } = new List<Settlement>();
		public List<Road> Roads { get; private set; } = new List<Road>();

		public static Result<Player> Create(string name) 
		{
			if (string.IsNullOrWhiteSpace(name))
				return Result<Player>.Failure("Player name cannot be empty.");
			return Result<Player>.Success(new Player(name));
		}

		public bool HasResources(Buyables buyable)
		{
			var costs = GameInfo.Costs[buyable];
			foreach (var (resource, required) in costs)
			{
				if (ResourceCount[resource] < required)
					return false;
			}
			return true;
		}

	}
}
