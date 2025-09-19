var builder = WebApplication.CreateBuilder();

builder.Services.AddTransient<ITimeService, ShortTimeService>();
builder.Services.AddTransient<TimeMessage>();

var app = builder.Build();

app.Run(async context =>
{
    var timeMessage = context.RequestServices.GetService<TimeMessage>();
    context.Response.ContentType = "text/html;charset=utf-8";
    await context.Response.WriteAsync($"<h2>{timeMessage?.GetTime()}</h2>");
});

app.Run();

class TimeMessage
{
    ITimeService timeService;
    public TimeMessage(ITimeService timeService)
    {
        this.timeService = timeService;
    }
    public string GetTime() => $"Time: {timeService.GetTime()}";
}
interface ITimeService
{
    string GetTime();
}
class ShortTimeService : ITimeService
{
    public string GetTime() => DateTime.Now.ToShortTimeString();
}