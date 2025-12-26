namespace Lingafon.Core.Interfaces.Services;

using System.Net.WebSockets;
using System;
using System.Collections.Generic;

public interface IOnlineStatusService
{
    bool IsOnline(Guid userId);
    void Add(Guid userId, WebSocket socket);
    void Remove(Guid userId);
    IReadOnlyDictionary<Guid, WebSocket> GetAllConnections();
}