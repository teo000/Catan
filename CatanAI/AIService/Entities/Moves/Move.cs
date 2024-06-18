using AIService.Entities.Data;

namespace AIService.Entities.Moves
{
    public abstract class Move
    {
        protected Move(Guid gameId) 
        {
            GameId = gameId;
        }
        public Guid GameId { get; set; }
        public MoveType MoveType { get; set; }
    }
}
