using AIService.Entities.Data;

namespace AIService.Utils
{
	public static class GameUtils
	{
		public static Dictionary<Resource, int> GetEmptyResourceDictionary()
		{
			var dict = new Dictionary<Resource, int>();
			foreach (Resource resource in Enum.GetValues(typeof(Resource)))
				if (resource != Resource.Desert)
				{
					dict.Add(resource, 0);
				}
			return dict;
		}
		
	}
}
