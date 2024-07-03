using AIService.Entities.Data;
using AIService.Entities.Game.GamePieces;

namespace AIService.Entities.Game.GameMap
{
    public class HexTile
    {
        public Resource Resource { get; set; }
        public int Number { get; set; }
        public List<Building> Buildings { get; set; } = new List<Building>();

		public int Value()
		{
			return 6 - Math.Abs(7 - Number);
		}
	}
}