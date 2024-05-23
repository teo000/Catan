using Catan.Domain.Data;
using FluentValidation;

namespace Catan.Application.Features.Trade.Commands.InitiateTrade
{
    public class InitiateTradeCommandValidator : AbstractValidator<InitiateTradeCommand>
    {
        public InitiateTradeCommandValidator()
        {
            RuleFor(x => x.ResourceToGive)
               .Must(BeAValidResource)
               .WithMessage("Invalid resource value provided.");

            RuleFor(x => x.CountToGive)
                .GreaterThan(0)
                .WithMessage("Number of resource to give must be positive");

            RuleFor(x => x.ResourceToReceive)
               .Must(BeAValidResource)
               .WithMessage("Invalid resource value provided.");

            RuleFor(x => x.CountToReceive)
                .GreaterThan(0)
                .WithMessage("Number of resource to receive must be positive");
        }

        private bool BeAValidResource(string resource)
        {
            return Enum.IsDefined(typeof(Resource), resource);
        }

    }
}
