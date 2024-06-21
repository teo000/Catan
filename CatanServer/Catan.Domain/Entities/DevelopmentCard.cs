using Catan.Domain.Data;

namespace Catan.Domain.Entities
{
	public class DevelopmentCard
	{
		public DevelopmentCard(DevelopmentType type, int roundDrawn) 
		{
			DevelopmentType = type;
			RoundDrawn = roundDrawn;
		}
		public int RoundDrawn { get; }
		public DevelopmentType DevelopmentType { get; }
	}
}
