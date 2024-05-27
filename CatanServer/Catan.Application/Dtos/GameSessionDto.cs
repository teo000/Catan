using Catan.Domain.Entities;

namespace Catan.Application.Dtos
{
    public class GameSessionDto
    {
        public Guid Id { get; set; }
        public MapDto? Map { get; set; }
        public List<PlayerDto> Players { get; set; }
		public PlayerDto TurnPlayer { get; set; }
        public string GameStatus { get; set; }
		//public int TurnPlayerIndex { get;  set; }
		public DateTime TurnEndTime { get;  set; }
        public int Round {  get; set; }
		public Dictionary<Guid, TradeDto> trades { get;  set; } 
        public DiceRollDto Dice {  get; set; }
	}
}
