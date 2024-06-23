// Add services to the container.
using Catan.API.Hubs;
using Catan.API.Notifiers;
using Catan.Application;
using Catan.Application.Contracts;
using Catan.Infrastructure;
using NLog;
using NLog.Web;


var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
	logger.Debug("init main");
	var builder = WebApplication.CreateBuilder(args);

	// Configure NLog
	builder.Logging.ClearProviders();
	builder.Host.UseNLog();
	builder.Services.AddSignalR();


	builder.Services.AddCors(options =>
	{
		options.AddPolicy("AllowAllHeaders", builder => builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
	});

	builder.Services.AddApplicationServices();
	builder.Services.AddInfrastructureToDI(builder.Configuration);

	builder.Services.AddControllers();
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();

	builder.Services.AddSingleton<IGameNotifier, SignalRGameNotifier>();


	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();
	app.UseCors("AllowAllHeaders");
	app.UseRouting();

	app.UseAuthorization();

	app.UseEndpoints(endpoints =>
	{
		endpoints.MapControllers();
		endpoints.MapHub<GameHub>("/gameHub");
		endpoints.MapHub<LobbyHub>("/lobbyHub");
	});

	app.Run();
}
catch (Exception exception)
{
	logger.Error(exception, "Stopped program because of exception");
	throw;
}
finally
{
	NLog.LogManager.Shutdown();
}
