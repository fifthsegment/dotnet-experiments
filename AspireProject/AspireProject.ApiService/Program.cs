using Npgsql;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.AddNpgsqlDataSource("Todos");
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

app.MapGet("/todos", async (NpgsqlConnection db) =>
{
    const string sql = @"
        SELECT Id, Title, IsComplete
        FROM Todos";

    var todos = await db.QueryAsync<Todo>(sql);
    if (todos != null && todos.Any())
    {
        return Results.Ok(todos);
    }
    else
    {
        return Results.NotFound();
    }
});


app.MapGet("/todos/{id}", async (int id, NpgsqlConnection db) =>
{
    const string sql = """
        SELECT Id, Title, IsComplete
        FROM Todos
        WHERE Id = @id
        """;
    
    return await db.QueryFirstOrDefaultAsync<Todo>(sql, new { id }) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound();
});

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public record Todo(int Id, string Title, bool IsComplete);