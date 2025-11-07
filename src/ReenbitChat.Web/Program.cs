using Microsoft.EntityFrameworkCore;
using ReenbitChat.Infrastructure;
using ReenbitChat.Web.Endpoints;
using ReenbitChat.Web.Hubs;
using System;

var builder = WebApplication.CreateBuilder(args);

// --- Services ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR (In-App, без Azure)
builder.Services.AddSignalR();

// EF Core
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(3)
    ));
// CORS
var corsPolicy = "_reenbitCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
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

// --- Middleware ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors(corsPolicy);

// --- Map Hubs & Controllers ---
app.MapMessages();
app.MapHub<ChatHub>("/hubs/chat");
app.MapControllers();

// Health check
app.MapGet("/api/health", () => Results.Ok(new { ok = true, ts = DateTime.UtcNow }));

app.Run();
