using DockerDemoBackendApi.Data;
using DockerDemoBackendApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DockerDemoBackendApi.Services;

public class MessageService : IMessageService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<MessageService> _logger;

    public MessageService(AppDbContext dbContext, ILogger<MessageService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(CancellationToken ct = default)
    {
        return await _dbContext.Messages
            .AsNoTracking()
            .OrderByDescending(m => m.Id)
            .ToListAsync(ct);
    }

    public async Task<Message> AddMessageAsync(string text, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Message text cannot be empty.", nameof(text));
        }

        var message = new Message { Text = text };

        _dbContext.Messages.Add(message);

        try
        {
            await _dbContext.SaveChangesAsync(ct);
            _logger.LogInformation("Added new message with ID {MessageId}", message.Id);

            return message;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while adding a new message.");
            throw new InvalidOperationException("Could not add the message to the database.", ex);
        }
    }

    public async Task<bool> DeleteMessageAsync(int id, CancellationToken ct = default)
    {
        var message = await _dbContext.Messages.FirstOrDefaultAsync(m => m.Id == id, ct);

        if (message is null)
        {
            _logger.LogWarning("Attempted to delete non-existent message with ID {MessageId}", id);
            return false;
        }

        _dbContext.Messages.Remove(message);

        try
        {
            await _dbContext.SaveChangesAsync(ct);
            _logger.LogInformation("Deleted message with ID {MessageId}", id);
            return true;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting message with ID {MessageId}.", id);
            throw new InvalidOperationException($"Could not delete the message with ID {id} from the database.", ex);
        }
    }
}
