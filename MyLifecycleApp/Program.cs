var builder = WebApplication.CreateBuilder(args);

// Жизненный цикл сервисов:
//builder.Services.AddTransient<ICounter, RandomCounter>();
//builder.Services.AddScoped<ICounter, RandomCounter>();
builder.Services.AddSingleton<ICounter, RandomCounter>();

builder.Services.AddSingleton<CounterService>();

var app = builder.Build();

app.UseMiddleware<CounterMiddleware>();

app.Run();
