using Catan.Application.Models.Identity;

namespace Catan.Application.Contracts
{
	public interface IAuthService
	{
		Task<(int, string)> Registration(RegistrationModel model, string role);
		Task<(int, string)> Login(LoginModel model);
		Task<(int, string)> Logout();
	}
}
