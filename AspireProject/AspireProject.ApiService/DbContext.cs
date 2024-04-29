using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AspireProject.ApiService;


public class AspireDbContext(DbContextOptions<AspireDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos { get; set; }
}

public class Todo
{
    [JsonPropertyName("Id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [JsonPropertyName("ExternalId")]
    public required string ExternalId { get; set; }

    [JsonPropertyName("Title")]
    public string? Title {get; set;}

    // Parameterized constructor
    [JsonConstructor]
    public Todo(Guid id, string title, string externalId)
    {
        Id = id;
        Title = title;
        ExternalId = externalId;
    }
}
