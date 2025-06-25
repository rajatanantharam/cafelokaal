using Microsoft.AspNetCore.SignalR;

namespace CafeLokaal.ConsumerApp.Hubs
{
    // This class represents a SignalR hub for the CafeLokaal application.
    // It allows clients to send and receive messages in real-time.
    // The hub can be used to notify clients about events such as order updates.
    public class CafeLokaalHub : Hub
    {
        public async Task SendMessage(string userId, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", userId, message);
        }

    }
}