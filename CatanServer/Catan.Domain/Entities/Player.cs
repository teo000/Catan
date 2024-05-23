using Catan.Domain.Common;
using Catan.Domain.Data;

namespace Catan.Domain.Entities
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
			ResourceCount = new Dictionary<Resource, int>();
			foreach (Resource resource in Enum.GetValues(typeof(Resource)))
			{
				ResourceCount.Add(resource, 0);
			}
		}

		public Guid Id { get; private set; }
		public string Name { get; private set; }
		public bool IsActive { get; private set; }
		public Dictionary<Resource, int> ResourceCount { get; private set; }
		public List<Settlement> Settlements { get; private set; } = new List<Settlement>();
		public List<Road> Roads { get; private set; } = new List<Road>();

		//ar trebui poate sa notez undeva ce fel de trade-uri din astea mai favorabile pot sa fac
		public static Result<Player> Create(string name) 
		{
			if (string.IsNullOrWhiteSpace(name))
				return Result<Player>.Failure("Player name cannot be empty.");
			return Result<Player>.Success(new Player(name));
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

		public bool HasResource(Resource resource, int count)
		{
			if (ResourceCount[resource] < count)
				return false;
			return true;
		}

		public void AssignResources (Dictionary<Resource, int> resourcesToAdd)
		{
			foreach (var (resource, count) in ResourceCount)
			{
				ResourceCount[resource] += resourcesToAdd[resource];
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
			int points = 0;
			foreach (var settlement in Settlements)
				points += settlement.IsCity ? 2 : 1;

			//adauga aia cu cel mai lung drum !!!!!!

			return points;
		}
	}
}
