using Catan.Domain.Entities.GamePieces;

namespace Catan.Domain.Entities
{
    public class City : Building
	{
		public City(Player player, int position) : base(player, position)
		{
		}

		public override int Points { get; } = 2;

	}
}
