namespace AIService.Entities.Common
{
	public static class RandomExtensions
	{
		private static readonly Random Random = new Random();

		public static T GetRandomElement<T>(this List<T> list)
		{
			int index = Random.Next(list.Count);
			return list[index];
		}

		public static T SelectWeightedRandom<T>(Dictionary<T, int> itemsWithWeights) where T : notnull
		{
			int totalWeight = itemsWithWeights.Values.Sum();

			int randomNumber = Random.Next(totalWeight);

			foreach (var item in itemsWithWeights)
			{
				if (randomNumber < item.Value)
				{
					return item.Key;
				}
				randomNumber -= item.Value;
			}

			return default;
		}
	}
}
