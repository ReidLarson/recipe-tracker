using RecipeTracker.Api.Minimal.Endpoints.Internal;
using RecipeTracker.Core.Data;
using RecipeTracker.Core.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new SqliteConnectionFactory(builder.Configuration.GetValue<string>("ConnectionStrings:Sqlite")!));
builder.Services.AddSingleton<DatabaseInitializer>();

builder.Services.AddScoped<IRecipesRepository, RecipesRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseEndpoints<Program>();

var dbInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await dbInitializer.InitializeAsync();

app.Run();