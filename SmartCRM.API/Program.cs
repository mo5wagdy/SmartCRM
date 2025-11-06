using Microsoft.EntityFrameworkCore;
using SmartCRM.Infrastructure.DependencyInjection;
using SmartCRM.Infrastructure.Data;
using SmartCRM.Infrastructure.Data.Seed;

var builder = WebApplication.CreateBuilder(args);
// Add DbContext.
builder.Services.AddDbContext<CrmDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add Infrastructure dependencies
builder.Services.AddInfrastructure();
// Add Application dependencies
builder.Services.AddApplication();

var app = builder.Build();

// apply migrations and seed (safe in dev; adapt for prod)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CrmDbContext>();
    db.Database.Migrate(); // ensures DB is up to date
    await DataSeeder.SeedAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();