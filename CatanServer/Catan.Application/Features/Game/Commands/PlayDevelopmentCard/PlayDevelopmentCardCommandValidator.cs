using AutoMapper;
using Catan.Application.GameManagement;
using Catan.Domain.Data;
using FluentValidation;

namespace Catan.Application.Features.Game.Commands.PlayDevelopmentCard;

public class PlayDevelopmentCardCommandValidator : AbstractValidator<PlayDevelopmentCardCommand>
{
	public PlayDevelopmentCardCommandValidator()
	{
		RuleFor(x => x.DevelopmentType)
		   .Must(BeAValidDevelopmentType)
		   .WithMessage("Invalid move type provided.");
	}
	private bool BeAValidDevelopmentType(string developmentType)
	{
		return Enum.IsDefined(typeof(DevelopmentType), developmentType);
	}
}
