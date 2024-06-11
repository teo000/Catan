using AIService.Entities.Game.GamePieces;
using AIService.Entities.Game.Harbors;

namespace AIService.Entities.Game.GameMap
{
    public class Map
    {
        public HexTile[] HexTiles { get; set; }
        public int ThiefPosition { get; set; }
        public Settlement[] Settlements { get; set; }
        public City[] Cities { get; set; }
        public Road[] Roads { get; set; }
        public SpecialHarbor[] SpecialHarbors { get; set; }
    }
}