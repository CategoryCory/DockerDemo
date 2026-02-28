namespace DockerDemoBackendApi.Models;

public sealed class Message
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;
}
