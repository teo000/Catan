using Catan.Domain.Common;

namespace Catan.Domain.Entities
{
	public class Player
	{
		private Player(string name)
		{
			Id = Guid.NewGuid();
			Name = name;
			IsActive = true;
		}

		public Guid Id { get; private set; }
		public string Name { get; private set; }
		public bool IsActive { get; private set; }

		public static Result<Player> Create(string name) 
		{
			if (string.IsNullOrWhiteSpace(name))
				return Result<Player>.Failure("Player name cannot be empty.");
			return Result<Player>.Success(new Player(name));
		}
	}
}
