using Catan.Domain.Data;
using Catan.Domain.Entities;

namespace Catan.Application.Dtos
{
	public class PlayerDto
	{
		public Guid Id { get; private set; }
		public string Name { get; private set; }
		public bool IsActive { get; private set; }
		public Dictionary<Resources, int> ResourceCount { get; private set; }
		public List<Settlement> Settlements { get; private set; } = new List<Settlement>();
		public List<Road> Roads { get; private set; } = new List<Road>();
	}
}
