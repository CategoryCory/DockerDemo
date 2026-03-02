using DockerDemoBackendApi.Dtos;
using DockerDemoBackendApi.Models;
using DockerDemoBackendApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DockerDemoBackendApi.Controllers;

/// <summary>
/// Controller for managing messages. Provides endpoints to retrieve, add, and delete messages.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    /// <summary>
    /// The message service used to perform operations related to messages.
    /// </summary>
    private readonly IMessageService _messageService;

    /// <summary>
    /// The logger used for logging information, warnings, and errors in the controller.
    /// </summary>
    private readonly ILogger<MessageController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageController"/> class.
    /// </summary>
    /// <param name="messageService">The message service to be used by the controller.</param>
    /// <param name="logger">The logger to be used by the controller.</param>
    public MessageController(IMessageService messageService, ILogger<MessageController> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all messages. This endpoint returns a list of messages in the system.
    /// </summary>
    /// <param name="ct">The cancellation token to cancel the operation.</param>
    /// <returns>A list of messages.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Message>))]
    public async Task<IActionResult> GetMessage(CancellationToken ct)
    {
        var messages = await _messageService.GetMessagesAsync(ct);
        return Ok(messages);
    }

    /// <summary>
    /// Adds a new message. This endpoint accepts a request body containing the text of the message to be added.
    /// </summary>
    /// <param name="addMessageRequest">The request object containing the text of the message to be added.</param>
    /// <param name="ct">The cancellation token to cancel the operation.</param>
    /// <returns>The created message.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Message))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddMessage([FromBody] AddMessageRequest addMessageRequest, CancellationToken ct)
    {
        try
        {
            var message = await _messageService.AddMessageAsync(addMessageRequest.Text, ct);
            return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid message text received.");
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to add message.");
            return StatusCode(500, new { error = "Failed to add the message." });
        }
    }

    /// <summary>
    /// Deletes a message by its ID. This endpoint removes the message with the specified ID from the system.
    /// </summary>
    /// <param name="id">The ID of the message to be deleted.</param>
    /// <param name="ct">The cancellation token to cancel the operation.</param>
    /// <returns>No content if successful, not found if the message doesn't exist.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteMessage(int id, CancellationToken ct)
    {
        try
        {
            var success = await _messageService.DeleteMessageAsync(id, ct);

            if (!success)
            {
                return NotFound(new { error = $"Message with ID {id} not found." });
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting message with ID {MessageId}.", id);
            return StatusCode(500, new { error = "Failed to delete the message." });
        }
    }
}
