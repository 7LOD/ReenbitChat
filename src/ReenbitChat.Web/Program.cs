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
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
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
app.MapGet("/api/health", () => Results.Ok(new { ok = true, ts = DateTime.UtcNow }));
app.MapGet("/__version", () =>
{
    var asm = typeof(Program).Assembly;
    return Results.Ok(new
    {
        asm.GetName().Name,
        asm.GetName().Version,
        env = app.Environment.EnvironmentName
    });
});

app.MapGet("/__routes", (IEnumerable<EndpointDataSource> sources) =>
{
    var routes = sources.SelectMany(s => s.Endpoints)
                        .OfType<RouteEndpoint>()
                        .Select(e => e.RoutePattern.RawText)
                        .OrderBy(x => x);
    return Results.Ok(routes);
});

app.MapGet("/api/health/full", () => Results.Ok(new { ok = true, utc = DateTime.UtcNow }))
   .WithName("HealthFull");
app.Run();

