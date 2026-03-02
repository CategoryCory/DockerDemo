using DockerDemoBackendApi.Data;
using DockerDemoBackendApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DockerDemoBackendApi.Services;

/// <summary>
/// Implementation of the <see cref="IMessageService"/> interface, providing methods for managing messages in the database.
/// </summary>
public class MessageService : IMessageService
{
    /// <summary>
    /// The application's database context, used for accessing and managing the messages stored in the database.
    /// </summary>
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// The logger used for logging information, warnings, and errors in the service.
    /// </summary>
    private readonly ILogger<MessageService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageService"/> class.
    /// </summary>
    /// <param name="dbContext">The application's database context.</param>
    /// <param name="logger">The logger instance for logging messages.</param>
    public MessageService(AppDbContext dbContext, ILogger<MessageService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all messages from the system. This method returns an enumerable collection of messages,
    /// allowing the caller to iterate through the messages as needed.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>An enumerable collection of messages.</returns>
    public async Task<IEnumerable<Message>> GetMessagesAsync(CancellationToken ct)
    {
        return await _dbContext.Messages
            .AsNoTracking()
            .OrderByDescending(m => m.Id)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Adds a new message with the specified text. This method validates the input text and attempts
    /// to add a new message to the database. If the operation is successful, the newly created message
    /// is returned. If the operation fails, an exception is thrown.
    /// </summary>
    /// <param name="text">The text of the message to be added.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The newly created message.</returns>
    /// <exception cref="ArgumentException">Thrown when the text is null, empty, or whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the message cannot be saved to the database.</exception>
    public async Task<Message> AddMessageAsync(string text, CancellationToken ct)
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

    /// <summary>
    /// Deletes a message with the specified identifier. This method attempts to find the message with the given ID
    /// and remove it from the database. If the message is found and successfully deleted, the method returns true.
    /// If the message is not found, it returns false. If an error occurs during the deletion process, an exception is thrown.
    /// </summary>
    /// <param name="id">The identifier of the message to be deleted.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>True if the message was successfully deleted, otherwise false.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when an error occurs while deleting the message from the database.
    /// </exception>
    public async Task<bool> DeleteMessageAsync(int id, CancellationToken ct)
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
