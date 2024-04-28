using Npgsql;
using Dapper;
using Microsoft.EntityFrameworkCore;
using AspireProject.ApiService;

var builder = WebApplication.CreateBuilder(args);
// string connString = builder.Configuration.GetConnectionString("ConnectionStrings__Todos");

// Add service defaults & Aspire components.
builder.AddServiceDefaults();


// builder.AddNpgsqlDataSource("Todos");
builder.AddNpgsqlDbContext<AspireDbContext>("Todos");
// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.MapGet("/todos", async ( AspireDbContext context) =>
{
    var todo = await context.Todos.ToListAsync();
    if (todo == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(todo);
});


app.MapGet("/todos/{id}", async (int id, AspireDbContext context) =>
{
    var todo = await context.Todos.FindAsync(id);
    if (todo == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(todo);
});

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

