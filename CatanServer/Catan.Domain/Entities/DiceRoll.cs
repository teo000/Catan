namespace Catan.Domain.Entities
{
	public class DiceRoll
	{
		private Random rnd = new Random();

		public int[] Values { get; private set; } = [6, 6];
		public bool RolledThisTurn { get; set; } = false;
		public int GetSummedValue()
		{
			return Values[0] + Values[1];
		}
		public (int, int)? Roll()
		{
			Values[0] = rnd.Next(1, 7);
			Values[1] = rnd.Next(1, 7);

			return (Values[0], Values[1]);
		}



	}
}
