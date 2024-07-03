using System.Security.Claims;

namespace Catan.Application.Contracts
{
	public interface ICurrentUserService
	{
		string UserId { get; }
		ClaimsPrincipal GetCurrentClaimsPrincipal();
		string GetCurrentUserId();
	}
}
