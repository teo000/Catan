using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Game.Queries.GetGameState
{
    public record GetGameState(Guid Id, Guid PlayerId) : IRequest<GameSessionResponse>;

}
