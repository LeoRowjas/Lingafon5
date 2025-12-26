using System.Net.WebSockets;
using Lingafon.Core.Interfaces.Repositories;
using Lingafon.Core.Interfaces.Services;
using Lingafon.Infrastructure.Services;

namespace Lingafon.API.WebSockets;

public class WebSocketHandler
{
    private readonly IOnlineStatusService _connectionManager;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public WebSocketHandler(
        IOnlineStatusService connectionManager,
        IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _connectionManager = connectionManager;
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task HandleAsync(HttpContext context, WebSocket socket)
    {
        var token = context.Request.Query["access_token"].ToString();
        if (string.IsNullOrEmpty(token))
            throw new Exception("No access_token provided in query string");
        var userId = _jwtTokenService.GetUserId(token);

        _connectionManager.Add(userId, socket);

        // Уведомить о статусе онлайн
        await BroadcastStatusAsync(userId, "online");

        // Обновить LastSeenAt при подключении
        var user = await _userRepository.GetByIdAsync(userId);
        if (user != null)
        {
            user.LastSeenAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
        }

        await ListenAsync(socket);

        _connectionManager.Remove(userId);

        // Уведомить о статусе оффлайн
        await BroadcastStatusAsync(userId, "offline");

        // Обновить LastSeenAt при отключении
        user = await _userRepository.GetByIdAsync(userId);
        if (user != null)
        {
            user.LastSeenAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
        }
    }

    private async Task BroadcastStatusAsync(Guid userId, string status)
    {
        var message = $"{{\"userId\":\"{userId}\",\"status\":\"{status}\"}}";
        foreach (var socket in _connectionManager.GetAllConnections())
        {
            if (socket.Value.State == WebSocketState.Open)
            {
                var buffer = System.Text.Encoding.UTF8.GetBytes(message);
                await socket.Value.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    private async Task ListenAsync(WebSocket socket)
    {
        var buffer = new byte[1024 * 4];
        try
        {
            WebSocketReceiveResult result;
            do
            {
                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                // Handle received message...
            } while (!result.CloseStatus.HasValue);
        }
        catch (Exception)
        {
            // Handle exception...
        }
        finally
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }
    }
}