using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReenbitChat.Infrastructure;
using ReenbitChat.Web.Endpoints;
using ReenbitChat.Web.Hubs;
using ReenbitChat.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Services ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<SentimentService>();

// SignalR (In-App, без Azure)
builder.Services.AddSignalR();

// EF Core
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(3)
    ));
// CORS
var corsPolicy = "AllowFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy.WithOrigins(
            "https://victorious-glacier-082ff5403.3.azurestaticapps.net",
            "https://reenbitchat-server-c0adandqbxdcczbw.westeurope-01.azurewebsites.net",
            "http://localhost:4200"
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

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(corsPolicy);

app.UseAuthorization();
// --- Map Hubs & Controllers ---
app.MapControllers().RequireCors(corsPolicy);
app.MapHub<ChatHub>("/hubs/chat").RequireCors(corsPolicy);
app.MapMessages();


// Health check
app.MapGet("/api/test-sentiment", async ([FromQuery] string text, [FromServices] SentimentService service) =>
{
    var result = await service.AnalizyAsync(text);
    return Results.Ok(new { text, result });
})
.WithName("TestSentiment")
.WithOpenApi();
app.MapGet("/api/health/full", async (SentimentService sentiment, AppDbContext db) =>
{
    var dbOk = await db.Messages.AnyAsync();
    var test = await sentiment.AnalizyAsync("I am happy");
    return Results.Ok(new
    {
        db = dbOk ? "ok" : "fail",
        sentiment = test.ToString(),
        time = DateTime.UtcNow
    });
});
app.Run();

