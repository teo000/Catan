using Catan.Application.Dtos.GamePieces;
using Catan.Domain.Entities.GamePieces;

namespace Catan.Application.Dtos
{
    public class MapDto
	{
		public HexTileDto[] HexTiles { get; set; }
		public int ThiefPosition { get; set; }
		public List<SettlementDto> Settlements {get; set;}
		public List<CityDto> Cities { get; set; }
		public List<RoadDto> Roads { get; set; }
		public List<SpecialHarborDto> SpecialHarbors { get; set; }
		
	}
}
