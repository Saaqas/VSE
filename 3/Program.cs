using System.Text;

var builder = WebApplication.CreateBuilder();

var services = builder.Services;

var app = builder.Build();
app.Map("/3.1", async context =>
{
    var sb = new StringBuilder();
    sb.Append("<h1>Все сервисы</h1>");
    sb.Append("<table>");
    sb.Append("<tr><th>Тип</th><th>Lifetime</th><th>Реализация</th></tr>");
    foreach (var svc in services)
    {
        sb.Append("<tr>");
        sb.Append($"<td>{svc.ServiceType.FullName}</td>");
        sb.Append($"<td>{svc.Lifetime}</td>");
        sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
        sb.Append("</tr>");
    }
    sb.Append("</table>");
    context.Response.ContentType = "text/html;charset=utf-8";
    await context.Response.WriteAsync(sb.ToString());
});
app.Map("/3.2", async context =>
{
var timeService = app.Services.GetService<ITimeService>();
await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");

}); app.Map("/3.3", async context =>
{
var timeMessage = context.RequestServices.GetService<MyTimeMessage>();
context.Response.ContentType = "text/html; charset=utf-8";
await context.Response.WriteAsync($"<h2>{timeMessage?.GetTime()}</h2>");
});

app.Run();

// ===== классы и интерфейсы для 3.3 =====

class MyTimeMessage
{
    IMyTimeService timeService;

    public MyTimeMessage(IMyTimeService timeService)
    {
        this.timeService = timeService;
    }

    public string GetTime() => $"Time: {timeService.GetTime()}";
}

interface IMyTimeService
{
    string GetTime();
}

class MyShortTimeService : IMyTimeService
{
    public string GetTime() => DateTime.Now.ToShortTimeString();
}

class MyLongTimeService : IMyTimeService
{
    public string GetTime() => DateTime.Now.ToLongTimeString();
}
interface ITimeService
{
    string GetTime();
}
class ShortTimeService : ITimeService
{
    public string GetTime() => DateTime.Now.ToShortTimeString();
}
class LongTimeService : ITimeService
{
    public string GetTime() => DateTime.Now.ToLongTimeString();
}
interface ILogger
{
    void Log(string message);
}
class Logger : ILogger
{
    public void Log(string message) => Console.WriteLine(message);
}
class Message
{
    ILogger logger;
    public string Text { get; set; } = "";
    public Message(ILogger logger)
    {
        this.logger = logger;
    }
    public void Print() => logger.Log(Text);
}