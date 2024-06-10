using Catan.Application.Contracts;
using Catan.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catan.Infrastructure
{
	public static class InfrastructureRegistrationDI
	{
		public static IServiceCollection AddInfrastructureToDI(
			this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpClient();

			services.AddScoped<IAIService, AIService>();
			services.AddSingleton(s => new Lazy<IAIService>(s.GetRequiredService<IAIService>));

			return services;
		}
	}
}
