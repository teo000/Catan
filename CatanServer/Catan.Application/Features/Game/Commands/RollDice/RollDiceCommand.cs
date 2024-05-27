using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Game.Commands.RollDice
{
    public class RollDiceCommand : IRequest<DiceRollResponse>
	{
		public Guid GameId { get; set; }
		public Guid PlayerId { get; set; }
	}
}
