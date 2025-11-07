using Microsoft.EntityFrameworkCore;
using ReenbitChat.Infrastructure;
using ReenbitChat.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR().AddAzureSignalR(builder.Configuration["Azure:SignalR:ConnectionString"]);

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var corsPolicy = "_reenbitCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "https://victorious-glacier-082ff5403.3.azurestaticapps.net")
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


app.MapHub<ChatHub>("/hubs/chat");

app.MapControllers();

app.MapGet("/api/health", () => Results.Ok(new { ok = true, ts = DateTime.UtcNow }));

app.Run();
