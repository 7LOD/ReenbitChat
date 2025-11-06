using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ReenbitChat.Application.Dtos;
using ReenbitChat.Domain;
using ReenbitChat.Infrastructure;

namespace ReenbitChat.Web.Hubs
{
    public class ChatHub(AppDbContext db) : Hub
    {
        public async Task JoinRoom(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
        }

        public async Task SendMessage(SendMessageRequest req)
        {
            var message = new Message
            {
                UserName = req.UserName,
                Text = req.Text,
                Room = string.IsNullOrWhiteSpace(req.Room) ? "general" : req.Room,
                CreatedAtUtc = DateTime.UtcNow,
            };
            db.Messages.Add(message);
            await db.SaveChangesAsync();

            var dto = new MessageDto(
                message.Id,
                message.UserName,
                message.Text,
                message.Room,
                message.CreatedAtUtc,
                (int)message.Sentiment
);
            await Clients.Group(message.Room).SendAsync("ReceiveMessage", dto);
        }
        public async Task<List<MessageDto>> History(string room, int take = 50)
        {
            room = string.IsNullOrWhiteSpace(room) ? "general" : room;
            return await db.Messages
                .Where(m => m.Room == room)
                .OrderByDescending(m => m.CreatedAtUtc)
                .Take(take)
                .Select(m => new MessageDto(m.Id, m.UserName, m.Text, m.Room, m.CreatedAtUtc, (int)m.Sentiment))
                .ToListAsync();
        }
    }
}
