using Catan.Application.Features.Game.Commands.PlaceSettlement;
using Catan.Domain.Data;
using FluentValidation;

namespace Catan.Application.Features.Game.Commands.PlaceCity
{
	public class PlaceCityCommandValidator : AbstractValidator<PlaceCityCommand>
	{
		public PlaceCityCommandValidator()
		{
			RuleFor(city => city.Position).InclusiveBetween(0, GameMapData.SETTLEMENTS_NO - 1);
		}
	}
}
