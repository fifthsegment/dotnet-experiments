using Microsoft.EntityFrameworkCore;

namespace AspireProject.ApiService;


public class AspireDbContext(DbContextOptions<AspireDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos { get; set; }
}

public class Todo
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string ExternalId { get; set; }

    public string? Title {get; set;}
}
