using Bridgenext.API.Extensions;
using Bridgenext.DataAccess;
using Microsoft.EntityFrameworkCore;

bool efMigrationMode = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("EF_MIGRATION_SCRIPT"));
var functionalTestMode = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("FUNCTIONAL_TESTS"));
var builder = WebApplication.CreateBuilder();
var configurationBuilder = builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddEnvironmentVariables().AddJsonFile($"appsettings.json", optional: true);

if (functionalTestMode)
{
    builder.Services.AddDbContext<UserSystemContext>(options => options.UseInMemoryDatabase("InMemoryDB"));
}

var configuration = configurationBuilder.Build();
builder.Services.AddSingleton(configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.RegisterRepositories();
builder.Services.RegisterEngines();
builder.Services.RegisterValidators();
builder.Services.RegisterDatabaseContext(configuration);
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapControllers();

if (!functionalTestMode) { app.UseHttpsRedirection(); }
app.MapHealthChecks("/Health");

await app.RunAsync();
