using Catan.Domain.Data;

namespace Catan.Domain.Entities.Harbors
{
	public class SpecialHarbor : Harbor
	{
		public SpecialHarbor(int position, Resource resource) : base(position)
		{
			Resource = resource;
		}
		public Resource Resource { get; private set; }
	}
}
