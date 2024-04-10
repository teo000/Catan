using Catan.Domain.Entities;

namespace Catan.Application.Dtos
{
	public class MapDto
	{
		public List<HexTileDto> hexTiles { get; set; }
		public int ThiefPosition { get; set; }
	}
}
