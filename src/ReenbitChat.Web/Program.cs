using Microsoft.EntityFrameworkCore;
using ReenbitChat.Application.Dtos;
using ReenbitChat.Infrastructure;
using ReenbitChat.Web.Endpoints;
using ReenbitChat.Web.Hubs;
using Microsoft.Azure.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSignalR();
}
else
{
    builder.Services.AddSignalR().
        AddAzureSignalR(builder.Configuration["Azure:SignalR:ConnectionString"]);
    builder.Services.AddSignalR(o => o.EnableDetailedErrors = true);
}

var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(conn));

var corsPolicy = "_reenbitCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy, policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "https://victorious-glacier-082ff5403.3.azurestaticapps.net"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();


app.UseCors(corsPolicy);

app.MapGet("api/messages/history", async (string room, AppDbContext db, int take = 50) =>
{
    Console.WriteLine("it is program.cs");
    room = string.IsNullOrWhiteSpace(room) ? "general" : room;

    var messages = await db.Messages
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

    return Results.Ok(messages);
});

app.MapGet("api/health", () => Results.Ok(new { ok = true, ts = DateTime.UtcNow }))
    .WithName("Health");


app.MapHub<ChatHub>("/hubs/chat", options =>
{
    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets |
                         Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
});

app.Run();