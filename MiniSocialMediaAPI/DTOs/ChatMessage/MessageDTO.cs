namespace MiniSocialMediaAPI.DTOs.ChatMessage
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public required string UserId { get; set; }
        public required string Text { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
