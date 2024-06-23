using Catan.Domain.Data;
using FluentValidation;

namespace Catan.Application.Features.Game.Commands.DiscardHalf
{
	public class DiscardHalfCommandValidator : AbstractValidator<DiscardHalfCommand>
	{
		public DiscardHalfCommandValidator() 
		{
			RuleForEach(x => x.ResourceCount.Keys)
				.Must(BeAValidResource)
				.WithMessage("Invalid resource type: {PropertyValue}");
			RuleForEach(x => x.ResourceCount.Values)
				.GreaterThanOrEqualTo(0)
				.WithMessage("Cannot discard a negative number.");
		}
		private bool BeAValidResource(string resource)
		{
			return Enum.IsDefined(typeof(Resource), resource);
		}
	}
}
