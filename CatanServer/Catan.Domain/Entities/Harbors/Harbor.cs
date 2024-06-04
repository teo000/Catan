namespace Catan.Domain.Entities.Harbors
{
	public class Harbor
    {
        public Harbor(int position) 
        { 
            Position = position;
        }
        public int Position { get; set; }
    }
}
