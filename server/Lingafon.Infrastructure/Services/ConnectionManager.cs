using System.Collections.Concurrent;
using System.Net.WebSockets;
using Lingafon.Core.Interfaces.Services;

namespace Lingafon.Infrastructure.Services;

public class ConnectionManager : IOnlineStatusService
{
    private readonly ConcurrentDictionary<Guid, WebSocket> _connections = new();

    public void Add(Guid userId, WebSocket socket)
        => _connections[userId] = socket;

    public void Remove(Guid userId)
        => _connections.TryRemove(userId, out _);

    public bool IsOnline(Guid userId)
        => _connections.ContainsKey(userId);

    public WebSocket? Get(Guid userId)
        => _connections.GetValueOrDefault(userId);

    public IReadOnlyDictionary<Guid, WebSocket> GetAllConnections() => _connections;
}