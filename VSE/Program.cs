using Microsoft.AspNetCore.Authentication;
using System;
using System.Text.RegularExpressions;
using static TokenMiddleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// ������� �������� (http://localhost:5000/) -> ����� ��������
app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync("<!DOCTYPE html><html><body></body></html>");
});

// �������� 1 (http://localhost:5000/welcome) -> ����������� WelcomePage
app.UseWelcomePage("/2.1");
List<Person1> users = new List<Person1>
{
    new() { Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37 },
    new() { Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 41 },
    new() { Id = Guid.NewGuid().ToString(), Name = "Sam", Age = 24 }
};
// �������� 2 (http://localhost:5000/hello) -> ����� ������
app.MapGet("/2.2", async context =>
{
    context.Response.ContentType = "text/plain; charset=utf-8";
    await context.Response.WriteAsync("Hello METANIT.COM");
});    
int x = 2;
app.MapGet("/2.3", async context =>
{
        x = x * 2;  //  2 * 2 = 4
        await context.Response.WriteAsync($"Result: {x}");

}
);
app.MapGet("/2.4", async context =>
{
    var response = context.Response;
    response.Headers.ContentLanguage = "ru-RU";
    response.Headers.ContentType = "text/plain; charset=utf-8";
   response.Headers.Append("secret-id", "256");    // ���������� ���������� ���������
   await response.WriteAsync("������ METANIT.COM");
});
app.MapGet("/2.5", async context =>
{
    context.Response.StatusCode = 404;
    await context.Response.WriteAsync("Resource Not Found");
});
app.MapGet("/2.6", async context =>
{
    var response = context.Response;
    response.ContentType = "text/html; charset=utf-8";
    await response.WriteAsync("<h2>Hello METANIT.COM</h2><h3>Welcome to ASP.NET Core</h3>");
});
app.MapGet("/2.7", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    var stringBuilder = new System.Text.StringBuilder("<table>");

    foreach (var header in context.Request.Headers)
    {
        stringBuilder.Append($"<tr><td>{header.Key}</td><td>{header.Value}</td></tr>");
    }
    stringBuilder.Append("</table>");
    await context.Response.WriteAsync(stringBuilder.ToString());
}
);
app.MapGet("/2.8", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    var stringBuilder = new System.Text.StringBuilder("<table>");

    foreach (var header in context.Request.Headers)
    {
        stringBuilder.Append($"<tr><td>{header.Key}</td><td>{header.Value}</td></tr>");
    }
    stringBuilder.Append("</table>");
    await context.Response.WriteAsync(stringBuilder.ToString());
});
app.MapGet("/2.9", async context =>
{
     await context.Response.WriteAsync($"Path: {context.Request.Path}");

});
app.MapGet("/2.10/{*subpath}", async context =>
{
    var path = context.Request.Path.Value;
    var now = DateTime.Now;
    var response = context.Response;

    if (path == "/2.10/date")
        await response.WriteAsync($"Date: {now.ToShortDateString()}");
    else if (path == "/2.10/time")
        await response.WriteAsync($"Time: {now.ToShortTimeString()}");
    else
        await response.WriteAsync("Hello METANIT.COM");
});

app.MapGet("/2.11", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync($"<p>Path: {context.Request.Path}</p>" +
        $"<p>QueryString: {context.Request.QueryString}</p>");
});
app.MapGet("/2.12", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    var stringBuilder = new System.Text.StringBuilder("<h3>��������� ������ �������</h3><table>");
    stringBuilder.Append("<tr><td>��������</td><td>��������</td></tr>");

    foreach (var param in context.Request.Query)
    {
        stringBuilder.Append($"<tr><td>{param.Key}</td><td>{param.Value}</td></tr>");
    }

    stringBuilder.Append("</table>");
    await context.Response.WriteAsync(stringBuilder.ToString());
});
app.MapGet("/2.13", async context =>
{
    string name = context.Request.Query["name"];
    string age = context.Request.Query["age"];
    await context.Response.WriteAsync($"{name} - {age}"); 
});
app.MapGet("/2.14", async context =>
{
    await context.Response.SendFileAsync("C:\\Users\\saqqa\\source\\repos\\VSE\\VSE\\Tigr.jpeg");
});
app.MapGet("/2.15", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("html/index.html");
});
app.MapGet("/2.16", async context =>
{
    var path = context.Request.Path;
    var fullPath = $"html/{path}";
    var response = context.Response;

    response.ContentType = "text/html; charset=utf-8";
    if (File.Exists(fullPath))
    {
        await response.SendFileAsync(fullPath);
    }
    else
    {
        response.StatusCode = 404;
        await response.WriteAsync("<h2>Not Found</h2>");
    }
});
app.MapGet("/2.17", async context =>
{
    context.Response.Headers.ContentDisposition = "attachment; filename=my_forest.jpg";
    await context.Response.SendFileAsync("forest.jpg");
});
app.MapGet("/2.18", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("html/index1.html");
});

// POST -> ������������ �������� �����
app.MapPost("/2.18/postuser", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";

    var form = await context.Request.ReadFormAsync();
    string name = form["name"];
    string age = form["age"];

    await context.Response.WriteAsync(
        $"<h2>���������</h2><p>Name: {name}</p><p>Age: {age}</p>");
});
app.MapGet("/2.19", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("html/index2.html");
});
app.MapPost("/2.19/postuser", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";

    var form = await context.Request.ReadFormAsync();
    string name = form["name"];
    string age = form["age"];
    string[] languages = form["languages"];

    // ���������� ��������� ����� � ������
    string langList = string.Join(", ", languages);

    await context.Response.WriteAsync(
        $"<h2>���������</h2>" +
        $"<p>Name: {name}</p>" +
        $"<p>Age: {age}</p>" +
        $"<p>Languages: {langList}</p>");
});

/////////////////////
app.MapGet("/2.20", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("html/index3.html");
});

// POST -> ������������ �������� �����
app.MapPost("/2.20/postuser", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";

    var form = await context.Request.ReadFormAsync();
    string name = form["name"];
    string age = form["age"];
    string[] languages = form["languages"];

    // ���������� ��������� ����� � ������
    string langList = string.Join(", ", languages);

    await context.Response.WriteAsync(
        $"<h2>���������</h2>" +
        $"<p>Name: {name}</p>" +
        $"<p>Age: {age}</p>" +
        $"<p>Languages: {langList}</p>");
});
app.MapGet("/2.21/{*subpath}", async context =>
{
    var subpath = context.GetRouteValue("subpath")?.ToString();
    if (subpath == "old")
    {
        await context.Response.WriteAsync("Old Page");
    }
    else
    {
        await context.Response.WriteAsync("Main Page");
    }
});
app.MapPost("/api/user", (Person person) =>
{
    var message = $"Name: {person.Name}  Age: {person.Age}";
    return Results.Json(new { text = message });
});

app.MapGet("/2.22", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("html/index4.html");
});
app.MapGet("/2.23", async context =>
{
    var response = context.Response;
    var request = context.Request;

    response.ContentType = "text/html; charset=utf-8";

    if (request.Path == "/upload" && request.Method == "POST")
    {
        IFormFileCollection files = request.Form.Files;
        // ���� � �����, ��� ����� ��������� �����
        var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
        // ������� ����� ��� �������� ������
        Directory.CreateDirectory(uploadPath);
        foreach (var file in files)
        {
            // ���� � ����� uploads
            string fullPath = $"{uploadPath}/{file.FileName}";

            // ��������� ���� � ����� uploads
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
        await response.WriteAsync("����� ������� ���������");
    }
    else
    {
        await response.SendFileAsync("html/index5.html");
    }
});
app.Use(async (context, next) =>
{
    context.Items["date"] = DateTime.Now.ToShortDateString();
    await next.Invoke();
    Console.WriteLine($"Current date: {context.Items["date"]}");
});
app.MapGet("/2.24", async (context) =>
{
    var date = context.Items["date"] as string ?? DateTime.Now.ToShortDateString();
    await context.Response.WriteAsync($"Date: {date}");
});
app.Use(async (context, next) =>
{
    string? path = context.Request.Path.Value?.ToLower();
    if (path == "/2.25/date")
    {
        await context.Response.WriteAsync($"Date: {DateTime.Now.ToShortDateString()}");
    }
    else
    {
        await next.Invoke();
    }
});
app.MapGet("/2.25", async context =>
{
    await context.Response.WriteAsync($"Hello METANIT.COM");
});
app.UseWhen(
    context => context.Request.Path == "/2.26/time", // ���� ���� ������� "/time"
    appBuilder =>
    {
        // ��������� ������ - ������� �� ������� ����������
        appBuilder.Use(async (context, next) =>
        {
            var time = DateTime.Now.ToShortTimeString();
            Console.WriteLine($"Time: {time}");
            await next();   // �������� ��������� middleware
        });
        // ���������� �����
        appBuilder.Run(async context =>
        {
            var time = DateTime.Now.ToShortTimeString();
            await context.Response.WriteAsync($"Time: {time}");
        });
    });
app.MapGet("/2.26", async context =>
{
    await context.Response.WriteAsync($"Hello METANIT.COM");
});
app.Map("/2.27/time", appBuilder =>
{
    var time = DateTime.Now.ToShortTimeString();

    // ��������� ������ - ������� �� ������� ����������
    appBuilder.Use(async (context, next) =>
    {
        Console.WriteLine($"Time: {time}");
        await next();   // �������� ��������� middleware
    });

    appBuilder.Run(async context => await context.Response.WriteAsync($"Time: {time}"));
});
app.MapGet("/2.27", async context =>
{
    await context.Response.WriteAsync($"Hello METANIT.COM");
});
app.Map("/2.28", appBuilder =>
{
    appBuilder.UseMiddleware<TokenMiddleware>();

    appBuilder.Run(async context =>
    {
        await context.Response.WriteAsync("Hello METANIT.COM");
    });
});

app.Run();

public record Person(string Name, int Age);
public class Person1
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
}

public class TokenMiddleware
{
    private readonly RequestDelegate next;

    public TokenMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
   
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Query["token"];
            if (string.IsNullOrWhiteSpace(token))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Not authenticated");
            }
            else
            {
                await _next(context);
            }
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Query["token"];
        if (token != "12345678")
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Token is invalid");
        }
        else
        {
            await next.Invoke(context);
        }
    }
    class Logger
    {
        public void Log(string message) => Console.WriteLine(message);
    }
    interface ILogger
    {
        void Log(string message);
    }
    class Logger1 : ILogger
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
}
