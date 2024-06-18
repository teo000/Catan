using Catan.Domain.Data;
using FluentValidation;

namespace Catan.Application.Features.Game.CommandsObsolete.PlaceSettlement
{
    public class PlaceSettlementCommandValidator : AbstractValidator<PlaceSettlementCommand>
    {
        public PlaceSettlementCommandValidator()
        {
            RuleFor(settlement => settlement.Position).InclusiveBetween(0, GameMapData.SETTLEMENTS_NO - 1);
        }
    }
}
