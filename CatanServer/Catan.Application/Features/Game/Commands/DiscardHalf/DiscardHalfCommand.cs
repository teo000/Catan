using Catan.Application.Responses;
using Catan.Domain.Data;
using MediatR;

namespace Catan.Application.Features.Game.Commands.DiscardHalf;

public class DiscardHalfCommand : IRequest<GameSessionResponse>
{
	public Guid GameId { get; set; }
	public Guid PlayerId { get; set; }
	public Dictionary<string, int> ResourceCount {  get; set; }
}
