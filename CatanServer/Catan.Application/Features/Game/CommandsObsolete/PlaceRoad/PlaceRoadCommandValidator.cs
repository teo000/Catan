using Catan.Domain.Data;
using FluentValidation;

namespace Catan.Application.Features.Game.CommandsObsolete.PlaceRoad
{
    public class PlaceRoadCommandValidator : AbstractValidator<PlaceRoadCommand>
    {
        public PlaceRoadCommandValidator()
        {
            RuleFor(road => road.Position).InclusiveBetween(0, GameMapData.ROADS_NO - 1);
        }
    }
}
