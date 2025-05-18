using api_teszt.Database;
using api_teszt.Helpers;
using api_teszt.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
bool ddl_drop = builder.Configuration.GetValue<bool>("dropOnLifecycleEnd");


String connectionString = builder.Configuration["PostgresConnection"]!;
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INotesService, NotesService>();
builder.Services
    .AddDbContext<DatabaseContext>(opt => { opt.UseNpgsql(connectionString); }).AddControllers();

var app = builder.Build();
var provider = app.Services;
var lifetime = app.Lifetime;
app.UseMiddleware<GlobalExceptionHandler>();
if (ddl_drop)
{
    lifetime.ApplicationStarted.Register(() =>
    {
        using var scope = provider.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        ctx.Database.EnsureCreated();
        Seeder.SeedDatabase(ctx);
    });
}


if (ddl_drop)
{
    lifetime.ApplicationStopped.Register(() =>
    {
        using var scope = provider.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        ctx.Database.EnsureDeleted();
    });
}

//app.UseHttpsRedirection();
app.MapControllers();

app.Run();