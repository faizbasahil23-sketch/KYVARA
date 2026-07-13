using Kyvara.Infrastructure.DependencyInjection;
using Kyvara.Infrastructure.Persistence;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMediatR(
    Assembly.Load("Kyvara.Application"));

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthentication();

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<KyvaraDbContext>();

    await Kyvara.Infrastructure.Persistence.Seed.DefaultAdminSeeder.SeedAsync(db);
}

app.Run();

