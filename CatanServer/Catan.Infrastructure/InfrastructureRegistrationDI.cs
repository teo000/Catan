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
			services.AddHttpClient<IAIService, AIService>(client =>
			{
				client.BaseAddress = new Uri("https://localhost:7295/api/ai/");
			});

			return services;
		}
	}
}
