using Microsoft.EntityFrameworkCore;
using RestaurantSimulation.Api;
using RestaurantSimulation.Application;
using RestaurantSimulation.Infrastructure;
using RestaurantSimulation.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPresentation(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

if (bool.Parse(builder.Configuration["SqlServer:AutomaticMigrations"]))
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<RestaurantSimulationContext>();
        db.Database.Migrate();
    }
}

if (bool.Parse(builder.Configuration["SqlServer:SeedDatabase"]))
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<RestaurantSimulationContext>();
        RestaurantSimulationContext.Seed(db);
    }
}

if (bool.Parse(builder.Configuration["UseSwagger"]))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api");
        c.OAuthClientId(builder.Configuration["Auth0:ClientId"]);
    });
}

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }