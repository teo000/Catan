namespace AIService.Entities.Common
{
	public static class ListExtensions
	{
		private static readonly Random Random = new Random();

		public static T GetRandomElement<T>(this List<T> list)
		{
			int index = Random.Next(list.Count);
			return list[index];
		}
	}
}
