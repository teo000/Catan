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

		public override string ToString()
		{
			return $"GameSessionDto {{\n" +
				   $"  Id = {Id},\n" +
				   $"  Map = {Map},\n" +
				   $"  Players = [\n    {string.Join(",\n    ", Players)}\n  ],\n" +
				   $"  TurnPlayer = {TurnPlayer},\n" +
				   $"  GameStatus = {GameStatus},\n" +
				   $"  TurnEndTime = {TurnEndTime},\n" +
				   $"  Round = {Round},\n" +
				   $"  Trades = [\n    {string.Join(",\n    ", Trades)}\n  ],\n" +
				   $"  Dice = {Dice},\n" +
				   $"  Winner = {Winner},\n" +
				   $"  LongestRoad = {LongestRoad},\n" +
				   $"  ThiefMovedThisTurn = {ThiefMovedThisTurn},\n" +
				   $"  DiscardedThisTurn = {DiscardedThisTurn}\n" +
				   $"}}";
		}
	}
}
