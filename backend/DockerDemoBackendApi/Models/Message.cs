namespace DockerDemoBackendApi.Models;

/// <summary>
/// Represents a message in the database.
/// </summary>
public sealed class Message
{
    /// <summary>
    /// Gets or sets the unique identifier for the message. This is the primary key in the database.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the text of the message. This is the content of the message that will be stored in the database.
    /// </summary>
    public string Text { get; set; } = string.Empty;
}
