namespace Catan.Domain.Entities.GamePieces
{
    public abstract class Building : GamePiece
    {
        protected Building(Player player, int position) : base(player, position)
        {
        }
        public abstract int Points { get; }
    }
}
