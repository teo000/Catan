namespace Catan.Domain.Entities
{
	public class Settlement : Building
	{
		public Settlement(Player player, int position) : base(player, position)
		{
		}
		public override int Points { get; } = 1;

	}
}
