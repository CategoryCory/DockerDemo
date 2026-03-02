using Microsoft.EntityFrameworkCore;
using DockerDemoBackendApi.Models;

namespace DockerDemoBackendApi.Data;

/// <summary>
/// The application's database context, responsible for managing the connection to the database and
/// providing access to the data models. This class inherits from <see cref="DbContext"/> and defines
/// a <see cref="DbSet{Message}"/> for managing messages in the database. 
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class with the specified options.
    /// </summary>
    /// <param name="options">The options to be used by the context.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{Message}"/> representing the collection of messages in the database.
    /// </summary>
    public DbSet<Message> Messages { get; set; }
}
