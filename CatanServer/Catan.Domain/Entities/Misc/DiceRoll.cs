namespace Catan.Domain.Entities
{
	public class DiceRoll
	{
		private Random rnd = new Random();

		public int[] Values { get; private set; } = [0, 0];
		public bool RolledThisTurn { get; private set; } = false;

		public int GetSummedValue()
		{
			return Values[0] + Values[1];
		}
		public (int, int)? Roll()
		{
			Values[0] = rnd.Next(1, 7);
			Values[1] = rnd.Next(1, 7);

			RolledThisTurn = true;

			return (Values[0], Values[1]);
		}

		public void Reset()
		{ 
			Values[0] = 0;
			Values[1] = 0;
			RolledThisTurn = false;
		}


	}
}
