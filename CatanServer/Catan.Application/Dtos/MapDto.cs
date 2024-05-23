using Catan.Domain.Entities;

namespace Catan.Application.Dtos
{
	public class MapDto
	{
		public HexTileDto[] HexTiles { get; set; }
		public int ThiefPosition { get; set; }
		public SettlementDto[] Settlements {get; set;}
		public RoadDto[] Roads { get; set; }

	}
}
