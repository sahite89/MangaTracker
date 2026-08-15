using MangaTracker.Application;
using MangaTracker.Infrastructure;
using MangaTracker.Infrastructure.Persistence;
using MangaTracker.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services
    .AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection("Jwt"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddOptions<DatabaseOptions>()
    .Configure<IConfiguration>((options, configuration) =>
    {
        options.ConnectionString =
            configuration.GetConnectionString("MangaTrackerConnectionString") ?? string.Empty;
    })
    .Validate(x => !string.IsNullOrWhiteSpace(x.ConnectionString))
    .ValidateOnStart();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(
    builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MangaTrackerDbContext>();
    context.Database.Migrate();

    await MangaTrackerDbInitializer.SeedAsync(context);
}

app.Run();