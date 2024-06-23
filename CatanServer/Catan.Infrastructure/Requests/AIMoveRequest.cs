using Catan.Application.Dtos;

namespace Catan.Infrastructure.Requests;

public class AIMoveRequest
{
	public GameSessionDto GameState { get; set; }
	public Guid PlayerId { get; set; }
}
