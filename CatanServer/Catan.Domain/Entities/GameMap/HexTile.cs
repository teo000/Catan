using Catan.Domain.Data;
using Catan.Domain.Entities.GamePieces;
using System.Text.Json.Serialization;

namespace Catan.Domain.Entities.GameMap;

public class HexTile
{
	public HexTile(Resource resource, int number)
	{
		Resource = resource;
		Number = number;
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public Resource Resource { get; private set; }
	public int Number {  get; private set; }
	public List<Building> Buildings { get; private set; } = new List<Building>();
}
