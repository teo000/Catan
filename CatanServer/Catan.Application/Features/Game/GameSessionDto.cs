using Catan.Domain.Entities;

namespace Catan.Application.Features.Game
{
    public class GameSessionDto
    {
        public Guid Id { get; set; }
        public Map? Map { get; set; }
        public List<Player> Players { get; set; }
        public string GameStatus { get; set; }
    }
}
