using System;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Map("/", () => "Index Page");
app.Map("/about", () => "About Page");
app.Map("/contact", () => "Contacts Page");
app.Map("/", () => "Index Page");
app.Map("/user", () => new Person("Tom", 37));
app.Map(
    "/users/{id}/{name}",
    (string id, string name) => $"User Id: {id}   User Name: {name}"
);
app.Map("/users", () => "Users Page");
app.Map("/", () => "Index Page");

app.Map("/users/{id}/{name}", HandleRequest);
app.Map("/users", () => "Users Page");
app.Map("/", () => "Index Page");

app.Map("/users/{id?}", (string? id) => $"User Id: {id ?? "Undefined"}");
app.Map("/", () => "Index Page");

app.Run();
string HandleRequest(string id, string name)
{
    return $"User Id: {id}   User Name: {name}";
}
record class Person(string Name, int Age);
