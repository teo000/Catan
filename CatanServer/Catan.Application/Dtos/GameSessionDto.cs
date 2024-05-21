using Catan.Domain.Entities;

namespace Catan.Application.Dtos
{
    public class GameSessionDto
    {
        public Guid Id { get; set; }
        public Map? Map { get; set; }
        public List<Player> Players { get; set; }
        public string GameStatus { get; set; }
		public int TurnPlayerIndex { get;  set; }
		public DateTime TurnEndTime { get;  set; }
	}
}
