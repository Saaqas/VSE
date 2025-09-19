public class CounterMiddleware
{
    private readonly RequestDelegate _next;
    private int _i = 0;

    public CounterMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, ICounter counter, CounterService counterService)
    {
        _i++;
        httpContext.Response.ContentType = "text/html; charset=utf-8";
        await httpContext.Response.WriteAsync(
            $"Запрос: {_i}; Counter: {counter.Value}; Service: {counterService.Counter.Value}"
        );
    }
}
