namespace Catan.API
{
	public class HexTile
	{
		public HexTile(Resource resource)
		{
			Resource = Enum.GetName(resource.GetType(), resource);
		}
		public string Resource { get; set; }
	}
}
