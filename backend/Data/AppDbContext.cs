using Microsoft.EntityFrameworkCore;
using DockerDemoBackendApi.Models;

namespace DockerDemoBackendApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Message> Messages { get; set; }
}
