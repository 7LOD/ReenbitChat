using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ReenbitChat.Infrastructure;
using ReenbitChat.Web.Endpoints;
using ReenbitChat.Web.Hubs;


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
    builder.Services.AddSignalR().AddAzureSignalR(builder.Configuration["Azure:SignalR:ConnectionString"]);
}


    var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(conn));



builder.Services.AddCors(o =>
{
    o.AddPolicy("ui", p => p
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins(
        builder.Configuration["Frontend:BaseUrl"] ?? "http://localhost:4200"
        ));
});


var app = builder.Build();

app.UseCors("ui");
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();


app.MapMessages();



app.MapGet("api/health", () => Results.Ok(new { ok = true, ts = DateTime.UtcNow} ))
    .WithName("Health");

if (builder.Environment.IsDevelopment())
{
    app.MapHub<ChatHub>("/hubs/chat");
}
else
{
   app.MapHub<ChatHub>("/hubs/chat");
}

app.Run();

