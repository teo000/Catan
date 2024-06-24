namespace Catan.API.Models
{
	public class CurrentUser
	{
		public bool IsAuthenticated { get; set; }
		public string Username { get; set; }
		public Dictionary<string, string> Claims { get; set; }
	}
}
