using Catan.Domain.Entities;

namespace Catan.Application.Dtos
{
	public class MapDto
	{
		public HexTileDto[] HexTiles { get; set; }
		public int ThiefPosition { get; set; }
		public List<SettlementDto> Settlements {get; set;}
		public List<RoadDto> Roads { get; set; }
		
	}
}
