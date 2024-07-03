// Add services to the container.
using Catan.API.Hubs;
using Catan.API.Notifiers;
using Catan.API.Services;
using Catan.Application;
using Catan.Application.Contracts;
using Catan.Infrastructure;
using Catan.Identity;
using NLog;
using NLog.Web;
using Catan.API.Utility;
using Microsoft.OpenApi.Models;


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

	builder.Services.AddInfrastrutureIdentityToDI(builder.Configuration);
	builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

	builder.Services.AddApplicationServices();
	builder.Services.AddInfrastructureToDI(builder.Configuration); builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

	builder.Services.AddApplicationServices();
	builder.Services.AddInfrastructureToDI(builder.Configuration);

	builder.Services.AddControllers();
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();

	builder.Services.AddSingleton<IGameNotifier, SignalRGameNotifier>();

	builder.Services.AddSwaggerGen(c =>
	{
		c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
		{
			Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
			Name = "Authorization",
			In = ParameterLocation.Header,
			Type = SecuritySchemeType.ApiKey,
			Scheme = "Bearer"
		});

		c.AddSecurityRequirement(new OpenApiSecurityRequirement()
				  {
					{
					  new OpenApiSecurityScheme
					  {
						Reference = new OpenApiReference
						  {
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						  },
						  Scheme = "oauth2",
						  Name = "Bearer",
						  In = ParameterLocation.Header,

						},
						new List<string>()
					  }
					});

		c.SwaggerDoc("v1", new OpenApiInfo
		{
			Version = "v1",
			Title = "Catan API",

		});

		c.OperationFilter<FileResultContentTypeOperationFilter>();
	});

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
