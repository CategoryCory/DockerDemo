using DockerDemoBackendApi.Models;

namespace DockerDemoBackendApi.Services;

public interface IMessageService
{
    Task<Message> AddMessageAsync(string text, CancellationToken ct = default);
    Task<bool> DeleteMessageAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Message>> GetMessagesAsync(CancellationToken ct = default);
}
