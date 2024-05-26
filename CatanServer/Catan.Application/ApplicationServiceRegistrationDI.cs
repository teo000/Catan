using Catan.Application.Mapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Catan.Application
{
	public static class ApplicationServiceRegistrationDI
	{
		public static void AddApplicationServices(this IServiceCollection services)
		{
			services.AddMediatR
			(
				cfg => cfg.RegisterServicesFromAssembly(
					Assembly.GetExecutingAssembly())
			);

			services.AddSingleton<GameSessionManager>();
			services.AddSingleton<LobbyManager>();
			services.AddAutoMapper(typeof(MappingProfile).Assembly);

		}
	}
}
