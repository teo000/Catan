namespace Catan.Application
{
	public static class JoinCodeGenerator
	{
		private static readonly Random _random = new Random();
		private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

		public static string GenerateJoinCode(int length = 6)
		{
			return new string(Enumerable.Repeat(Characters, length)
										.Select(s => s[_random.Next(s.Length)]).ToArray());
		}
	}
}
