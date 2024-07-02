using AIService.Entities.Data;

namespace AIService.Entities.Common
{
	public static class ResourceCountUtils
	{
		public static int CardsNo(Dictionary<Resource, int> resourceCount)
		{
			int no = 0;
			foreach (var (resource, count) in resourceCount)
			{
				no += count;
			}
			return no;
		}
	}
}
