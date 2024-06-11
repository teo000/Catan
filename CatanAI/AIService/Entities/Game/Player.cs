using AIService.Entities.Data;
using AIService.Entities.Game.GamePieces;
using Catan.Entities.Data;

namespace AIService.Entities.Game
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Dictionary<Resource, int> ResourceCount { get; set; }
        public Dictionary<Resource, int> TradeCount { get; set; }
        public List<Settlement> Settlements { get; set; }
        public List<City> Cities { get; set; }
        public List<Road> Roads { get; set; }
        public Color Color { get; set; }
        public int LastPlacedSettlementPos { get; set; }
        public int WinningPoints { get; set; }
        public bool IsAI { get; set; }
    }
}