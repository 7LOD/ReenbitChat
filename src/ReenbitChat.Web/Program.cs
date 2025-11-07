using Microsoft.EntityFrameworkCore;
using ReenbitChat.Application.Dtos;
using ReenbitChat.Infrastructure;
using ReenbitChat.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSignalR(o => o.EnableDetailedErrors = true)
    .AddAzureSignalR(builder.Configuration["Azure:SignalR:ConnectionString"]);

var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(conn, b => b.MigrationsAssembly("ReenbitChat.Infrastructure")));

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors(corsPolicy);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/hubs/chat");
    endpoints.MapGet("/api/health", () => Results.Ok(new { ok = true, ts = DateTime.UtcNow }));
});

app.Run();
