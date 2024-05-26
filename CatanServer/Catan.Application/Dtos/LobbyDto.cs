using Catan.Domain.Entities;

namespace Catan.Application.Dtos
{
	public class LobbyDto
	{
		public Guid Id { get;  set; }
		public string JoinCode { get;  set; }
		public List<PlayerDto> Players { get;  set; } = new List<PlayerDto>();
		public GameSessionDto? GameSession { get;  set; }
	}
}
