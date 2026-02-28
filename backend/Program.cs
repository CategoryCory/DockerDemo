
using Microsoft.EntityFrameworkCore;
using DockerDemoBackendApi.Data;
using DockerDemoBackendApi.Services;

namespace DockerDemoBackendApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null);
                }
            );
        });

        builder.Services.AddScoped<IMessageService, MessageService>();

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Ensure the database is created and seeded with initial data
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();

            if (!dbContext.Messages.Any())
            {
                dbContext.Messages.Add(new Models.Message
                {
                    Text = "Hello from PostgreSQL via EF Core in Docker!"
                });

                dbContext.SaveChanges();
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseAuthorization();

        app.Use(async (ctx, next) =>
        {
            using (var scope = app.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Handling request: {Method} {Path}", ctx.Request.Method, ctx.Request.Path);
            }
            
            await next();
        });

        app.MapControllers();

        app.Run();
    }
}
