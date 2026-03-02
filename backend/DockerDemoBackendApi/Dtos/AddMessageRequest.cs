namespace DockerDemoBackendApi.Dtos;

/// <summary>
/// Data Transfer Object (DTO) for adding a new message.
/// </summary>
public sealed class AddMessageRequest
{
    /// <summary>
    /// Gets or sets the text of the message to be added.
    /// </summary>
    public required string Text { get; set; } = string.Empty;
}
