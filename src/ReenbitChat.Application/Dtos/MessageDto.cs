
namespace ReenbitChat.Application.Dtos
{
    public record MessageDto(
        Guid id, 
        string UserName, 
        string Text, 
        string Room, 
        DateTime CreatedAtUtc, 
        int Sentiment,
        bool isSystem = false
        );
}
