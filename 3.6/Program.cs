var builder = WebApplication.CreateBuilder();

builder.Services.AddTransient<ITimer, Timer>();
builder.Services.AddScoped<TimeService>();

var app = builder.Build();

app.UseMiddleware<TimerMiddleware>();

app.Run();
public interface ITimer
{
    string Time { get; }
}
public class Timer : ITimer
{
    public Timer()
    {
        Time = DateTime.Now.ToLongTimeString();
    }
    public string Time { get; }
}
public class TimeService
{
    private ITimer timer;
    public TimeService(ITimer timer)
    {
        this.timer = timer;
    }
    public string GetTime() => timer.Time;
}
public class TimerMiddleware
{
    TimeService timeService;
    public TimerMiddleware(RequestDelegate next, TimeService timeService)
    {
        this.timeService = timeService;
    }

    public async Task Invoke(HttpContext context)
    {
        await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");
    }
}