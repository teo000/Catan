using Catan.Domain.Data;
using Catan.Domain.Entities;

namespace Catan.Application.Dtos
{
	public class HexTileDto
	{
		public string Resource { get; private set; }
		public int Number { get; private set; }
	}
}
