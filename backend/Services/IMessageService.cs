using DockerDemoBackendApi.Models;

namespace DockerDemoBackendApi.Services;

/// <summary>
/// Interface for the message service, defining the contract for operations related to messages.
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// Adds a new message with the specified text.
    /// </summary>
    /// <param name="text">The text of the message to be added.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The newly created message.</returns>
    Task<Message> AddMessageAsync(string text, CancellationToken ct);

    /// <summary>
    /// Deletes a message with the specified identifier. Returns true if the message was successfully
    /// deleted, or false if the message was not found.
    /// </summary>
    /// <param name="id">The identifier of the message to be deleted.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>True if the message was successfully deleted, otherwise false.</returns>
    Task<bool> DeleteMessageAsync(int id, CancellationToken ct);

    /// <summary>
    /// Retrieves all messages from the system. This method returns an enumerable collection of messages,
    /// allowing the caller to iterate through the messages as needed.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>An enumerable collection of messages.</returns>
    Task<IEnumerable<Message>> GetMessagesAsync(CancellationToken ct);
}
