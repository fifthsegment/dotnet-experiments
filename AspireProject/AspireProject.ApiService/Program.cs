using Microsoft.EntityFrameworkCore;
using AspireProject.ApiService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();


// builder.AddNpgsqlDataSource("Todos");
builder.AddNpgsqlDbContext<AspireDbContext>("Todos");
// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// var db = app.Services.GetService<AspireDbContext>();
// db.Database.Migrate();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// map weatherservice class to /weatherforecast
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

app.MapPost("/todos", async (HttpContext httpContext, AspireDbContext dbContext) =>
{
    app.Logger.LogInformation("Received request to create a new todo.");

    // using (StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8))
    // {
    //     // string queryKey = httpContext.Request.RouteValues["queryKey"].ToString();
    //     string jsonstring = await reader.ReadToEndAsync();
        
    //     // Save the todo to the database using your AspireDbContext.
    //     // For example:
    //     // var todo = new Todo { Text = jsonstring };
    //     // db.Todos.Add(todo);
    //     // await db.SaveChangesAsync();

    //     // return jsonstring;
    // }

    // Read the request body to get the new todo data
    var requestBody = await httpContext.Request.ReadFromJsonAsync<Todo>();

    if (requestBody != null)
    {
        app.Logger.LogInformation("Creating a new todo");
        app.Logger.LogInformation("Received new body with title : {body}", requestBody.Title);

        // Add the new todo to the database
        dbContext.Todos.Add(requestBody);
        await dbContext.SaveChangesAsync();

        // Return a success response
        httpContext.Response.StatusCode = StatusCodes.Status201Created;
    }
    else
    {
        // Return a bad request response
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
});


app.MapGet("/todos/{externalId}", async (string externalId, AspireDbContext context) =>
{
    var todo = await context.Todos.FirstOrDefaultAsync(t => t.ExternalId == externalId);
    if (todo == null)
    {
        return Results.NotFound($"Todo with external ID '{externalId}' not found.");
    }
    return Results.Ok(todo);
});

app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AspireDbContext>();
    db.Database.Migrate();
}

app.Run();



record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

