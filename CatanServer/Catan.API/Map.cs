using System;

namespace Catan.API
{
	public class Map
	{
		public Map()
		{
			Resource[] allValues = (Resource[])Enum.GetValues(typeof(Resource));

			Random random = new Random();

			tiles = Enumerable.Range(0, 19)
					.Select(_ => new HexTile(allValues[random.Next(allValues.Length)]))
					.ToList();
		}
		public List<HexTile> tiles { get; private set; }
	}
}
