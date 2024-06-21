using Catan.Application.Dtos.GamePieces;
using Catan.Domain.Data;

namespace Catan.Application.Dtos
{
    public class PlayerDto
	{
		public Guid Id { get;  set; }
		public string Name { get;  set; }
		public bool IsActive { get;  set; }
		public Dictionary<Resource, int> ResourceCount { get; set; }
		public Dictionary<Resource, int> TradeCount { get; set; }
		public List<SettlementDto> Settlements { get; set; } = new List<SettlementDto>();
		public List<CityDto> Cities { get; set; } = new List<CityDto>();
		public List<RoadDto> Roads { get; set; } = new List<RoadDto>();
		public List<DevelopmentCardDto> DevelopmentCards { get; set; }
		public string Color { get; set; }
		public int WinningPoints { get; set; }
		public bool DiscardedThisTurn {  get; set; }
	}
}
