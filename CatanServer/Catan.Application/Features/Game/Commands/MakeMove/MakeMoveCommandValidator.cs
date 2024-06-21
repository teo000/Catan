using Catan.Domain.Data;
using FluentValidation;

namespace Catan.Application.Features.Game.Commands.MakeMove
{
	public class MakeMoveCommandValidator : AbstractValidator<MakeMoveCommand>
	{
		public MakeMoveCommandValidator() 
		{
			RuleFor(x => x.MoveType)
			   .Must(BeAValidMoveType)
			   .WithMessage("Invalid move type provided.");
		}
		private bool BeAValidMoveType(string moveType)
		{
			return Enum.IsDefined(typeof(MoveType), moveType);
		}
	}
}
