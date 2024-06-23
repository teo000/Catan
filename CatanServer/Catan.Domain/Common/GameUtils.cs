using Catan.Domain.Data;
using System.Text;

namespace Catan.Domain.Common
{
	public static class GameUtils
	{
		private static Random rng = new Random();
		public static Dictionary<Resource, int> ConvertToResourceDictionary(Dictionary<string, int> resourceCount)
		{
			var resourceDict = GetEmptyResourceDictionary();

			foreach (var kvp in resourceCount)
			{
				if (Enum.TryParse<Resource>(kvp.Key, true, out var resource))
				{
					resourceDict[resource] = kvp.Value;
				}
				else
				{
					throw new ArgumentException($"Invalid resource type: {kvp.Key}");
				}
			}

			return resourceDict;
		}

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

		public static string PrintDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Dictionary Contents:");

			foreach (var kvp in dictionary)
			{
				sb.AppendLine($"{kvp.Key.ToString()} : {kvp.Value.ToString()}");
			}

			return sb.ToString();
		}

		
	}
}
