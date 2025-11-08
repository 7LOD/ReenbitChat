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
        private readonly SentimentService _sentimentService = sentimentService;
        private readonly AppDbContext _db = db;

        public async Task JoinRoom(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
        }

        public async Task SendMessage(SendMessageRequest req)
        {
            var room = string.IsNullOrWhiteSpace(req.Room) ? "general" : req.Room;
            var sentiment = await _sentimentService.AnalizyAsync(req.Text);

            var message = new Message
            {
                Id = Guid.NewGuid(),
                UserName = req.UserName,
                Text = req.Text,
                Room = room,
                CreatedAtUtc = DateTime.UtcNow,
                Sentiment = (Sentiment)sentiment
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            var dto = new MessageDto(
                message.Id,
                message.UserName,
                message.Text,
                message.Room,
                message.CreatedAtUtc,
                sentiment
            );

            await Clients.Group(room).SendAsync("ReceiveMessage", dto);
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
