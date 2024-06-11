using AIService.Entities.Data;
using AIService.Entities.Game.GamePieces;

namespace AIService.Entities.Game.GameMap
{
    public class HexTile
    {
        public Resource Resource { get; private set; }
        public int Number { get; private set; }
        public List<Building> Buildings { get; private set; } = new List<Building>();
    }
}