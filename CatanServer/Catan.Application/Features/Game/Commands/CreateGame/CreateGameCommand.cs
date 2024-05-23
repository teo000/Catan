using Catan.Application.Responses;
using Catan.Domain.Entities;
using MediatR;

namespace Catan.Application.Features.Game.Commands.CreateGame
{
    public class CreateGameCommand : IRequest<GameSessionResponse>
    {
        public List<string> playerNames { get; set; }
    }
}
