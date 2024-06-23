using AIService.Entities.Data;
using AIService.Entities.Game.GameMap;
using AIService.Entities.Game.Misc;
using AIService.Entities.Game.Trades;

namespace AIService.Entities.Game
{
	public class GameState
	{
		public Guid Id { get; set; }
		public Map Map { get; set; }
		public List<Player> Players { get; set; }
		public GameStatus GameStatus { get; set; }
		public int TurnPlayerIndex { get; set; }
		public DateTime TurnEndTime { get; set; }
		public int Round { get; set; }
		public DiceRoll Dice { get; set; }
		public List<PlayerTrade> Trades { get; set; }
		public Player? Winner { get; set; }
		public LongestRoad? LongestRoad { get; set; }
		public bool ThiefMovedThisTurn { get; set; }
	}
}
