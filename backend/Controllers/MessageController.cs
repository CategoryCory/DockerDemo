using DockerDemoBackendApi.Dtos;
using DockerDemoBackendApi.Models;
using DockerDemoBackendApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DockerDemoBackendApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly ILogger<MessageController> _logger;

    public MessageController(IMessageService messageService, ILogger<MessageController> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Message>))]
    public async Task<IActionResult> GetMessage(CancellationToken ct = default)
    {
        var messages = await _messageService.GetMessagesAsync(ct);
        return Ok(messages);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Message))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddMessage([FromBody] AddMessageRequest addMessageRequest, CancellationToken ct = default)
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

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteMessage(int id, CancellationToken ct = default)
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
