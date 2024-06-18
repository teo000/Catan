using Catan.Domain.Data;
using FluentValidation;

namespace Catan.Application.Features.Game.CommandsObsolete.PlaceCity
{
	public class PlaceCityCommandValidator : AbstractValidator<PlaceCityCommand>
    {
        public PlaceCityCommandValidator()
        {
            RuleFor(city => city.Position).InclusiveBetween(0, GameMapData.SETTLEMENTS_NO - 1);
        }
    }
}
