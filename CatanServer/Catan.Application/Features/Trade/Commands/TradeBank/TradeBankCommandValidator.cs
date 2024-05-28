using Catan.Domain.Data;
using FluentValidation;


namespace Catan.Application.Features.Trade.Commands.TradeBank
{
	public class TradeBankCommandValidator : AbstractValidator<TradeBankCommand>
	{
		public TradeBankCommandValidator()
		{
			RuleFor(x => x.ResourceToGive)
			   .Must(BeAValidResource)
			   .WithMessage("Invalid resource value provided.");

			RuleFor(x => x.ResourceToReceive)
			   .Must(BeAValidResource)
			   .WithMessage("Invalid resource value provided.");

			RuleFor(x => x.Count)
				.GreaterThan(0)
				.WithMessage("Number of resource to give must be positive");
		}
		private bool BeAValidResource(string resource)
		{
			return Enum.IsDefined(typeof(Resource), resource);
		}
	}
}
