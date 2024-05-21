using Catan.Application.Features.Game.Responses;
using MediatR;

namespace Catan.Application.Features.Game.Queries.GetGameState
{
    public record GetGameState(Guid Id) : IRequest<GameSessionResponse>;

}
