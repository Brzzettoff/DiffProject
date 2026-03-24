using DiffProject.Api.Dao;
using DiffProject.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<IDiffRepository, InMemoryRepository>();
builder.Services.AddScoped<DiffService>();

var app = builder.Build();
app.MapControllers();
app.Run();