using Catan.Domain.Data;
using Catan.Domain.Entities;

namespace Catan.Application.Dtos
{
	public class PlayerDto
	{
		public Guid Id { get;  set; }
		public string Name { get;  set; }
		public bool IsActive { get;  set; }
		public Dictionary<Resource, int> ResourceCount { get; set; }
		//public List<SettlementDto> Settlements { get; set; } = new List<SettlementDto>();
		public List<RoadDto> Roads { get; set; } = new List<RoadDto>();
		public string Color { get; set; }
	}
}
