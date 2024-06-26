
using System.Text.Json.Serialization;

namespace AspireProject.Web;

public class WeatherApiClient(HttpClient httpClient, ILogger<WeatherApiClient> logger)
{
    public async Task<Todo[]> GetTodosAsync(int maxItems = 10, CancellationToken cancellationToken = default) {
        List<Todo>? todos = null;

        await foreach (var todo in httpClient.GetFromJsonAsAsyncEnumerable<Todo>("/todos", cancellationToken))
        {
            if (todos?.Count >= maxItems)
            {
                break;
            }
            if (todo is not null)
            {
                todos ??= [];
                todos.Add(todo);
            }
        }

        return todos?.ToArray() ?? [];
    }

    public async Task AddTodoAsync(Todo newTodo, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Frontend : Sending request to create a new todo.");
        var response = await httpClient.PostAsJsonAsync("/todos", newTodo, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
    public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<WeatherForecast>? forecasts = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>("/weatherforecast", cancellationToken))
        {
            if (forecasts?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                forecasts ??= [];
                forecasts.Add(forecast);
            }
        }

        return forecasts?.ToArray() ?? [];
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public record Todo
{
    [JsonPropertyName("Id")]
    public Guid Id { get; init; }
    
    [JsonPropertyName("Title")]
    public string Title { get; init; }
    
    [JsonPropertyName("ExternalId")]
    public string ExternalId { get; init; }

    [JsonConstructor]
    public Todo(Guid id, string title, string externalId)
    {
        Id = id;
        Title = title;
        ExternalId = externalId;
    }
}

