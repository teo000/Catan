namespace Catan.Application.Dtos
{
	public class GameSessionDto
    {
        public Guid Id { get; set; }
        public MapDto? Map { get; set; }
        public List<PlayerDto> Players { get; set; }
		public PlayerDto TurnPlayer { get; set; }
        public string GameStatus { get; set; }
		public DateTime TurnEndTime { get;  set; }
        public int Round {  get; set; }
		public List<TradeDto> Trades { get;  set; } 
        public DiceRollDto Dice {  get; set; }
		public PlayerDto? Winner { get;  set; }
        public LongestRoadDto? LongestRoad { get; set; }
        public bool ThiefMovedThisTurn { get; set; }
		public bool DiscardedThisTurn { get; set; } 


	}
}
