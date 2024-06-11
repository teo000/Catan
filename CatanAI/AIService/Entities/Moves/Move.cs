namespace AIService.Entities.Moves
{
    public abstract class Move
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
    }
}
