using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ReenbitChat.Application.Dtos;
using ReenbitChat.Domain;
using ReenbitChat.Infrastructure;
using ReenbitChat.Web.Services;

namespace ReenbitChat.Web.Hubs
{
    public class ChatHub(AppDbContext db, SentimentService sentimentService) : Hub
    {
        private readonly SentimentService _sentimentService;
        private readonly AppDbContext _db = db;

        public async Task JoinRoom(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
        }

        public async Task SendMessage(SendMessageRequest req)
        {
            var sentiment = await _sentimentService.AnalizyAsync(req.Text);

            var message = new Message
            {
                Id = Guid.NewGuid(),
                UserName = req.UserName,
                Text = req.Text,
                Room = string.IsNullOrWhiteSpace(req.Room) ? "general" : req.Room,
                CreatedAtUtc = DateTime.UtcNow,
                Sentiment = sentiment,
            };


            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            await Clients.Group(req.Room).SendAsync("ReceiveMessage", message);

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
            return await _db.Messages
                .Where(m => m.Room == room)
                .OrderByDescending(m => m.CreatedAtUtc)
                .Take(take)
                .Select(m => new MessageDto(
                    m.Id,
                    m.UserName,
                    m.Text,
                    m.Room,
                    m.CreatedAtUtc,
                    (int)m.Sentiment))
                .ToListAsync();
        }
    }
}
