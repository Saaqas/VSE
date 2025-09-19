var builder = WebApplication.CreateBuilder();

builder.Services.AddTransient<ICounter, RandomCounter>();
builder.Services.AddTransient<CounterService>();

var app = builder.Build();

app.UseMiddleware<CounterMiddleware>();

app.Run();
public class CounterMiddleware
{
    RequestDelegate next;
    int i = 0; // счетчик запросов
    public CounterMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext, ICounter counter, CounterService counterService)
    {
        i++;
        httpContext.Response.ContentType = "text/html;charset=utf-8";
        await httpContext.Response.WriteAsync($"Запрос {i}; Counter: {counter.Value}; Service: {counterService.Counter.Value}");
    }


}
public interface ICounter
{
    int Value { get; }
}
public class RandomCounter : ICounter
{
    static Random rnd = new Random();
    private int _value;
    public RandomCounter()
    {
        _value = rnd.Next(0, 1000000);
    }
    public int Value
    {
        get => _value;
    }
}
public class CounterService
{
    public ICounter Counter { get; }
    public CounterService(ICounter counter)
    {
        Counter = counter;
    }
}