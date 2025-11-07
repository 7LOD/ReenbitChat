using Microsoft.EntityFrameworkCore;
using ReenbitChat.Infrastructure;

namespace ReenbitChat.Web.Endpoints
{
    public static class MessagesEndpoints
    {
        public static IEndpointRouteBuilder MapMessages(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/messages", async (string? room, int take, AppDbContext db) =>
            {
                room ??= "general";
                take = take <= 0 ? 50 : Math.Min(take, 200);
                var data = await db.Messages
                .Where(m => m.Room == room)
                .OrderByDescending(m => m.CreatedAtUtc)
                .Take(take)
                .ToListAsync();
                return Results.Ok(data);
            })
            .WithTags("Messages");
            return app;
        }
    }
}
