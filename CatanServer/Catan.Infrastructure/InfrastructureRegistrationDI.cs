using Catan.Application;
using Catan.Application.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catan.Infrastructure
{
	public static class InfrastructureRegistrationDI
	{
		public static IServiceCollection AddInfrastructureToDI(
			this IServiceCollection services, IConfiguration configuration)
		{



			return services;
		}
	}
}
