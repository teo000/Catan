using Catan.Application.Dtos.GamePieces;

namespace Catan.Application.Dtos
{
    public class LongestRoadDto
	{
		public List<RoadDto> Roads { get; set; }
		public PlayerDto Player { get; set; }
	}
}
