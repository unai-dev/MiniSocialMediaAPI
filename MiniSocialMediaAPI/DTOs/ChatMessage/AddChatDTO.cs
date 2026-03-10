namespace MiniSocialMediaAPI.DTOs.ChatMessage
{
    public class AddChatDTO
    {
        public required string UserId { get; set; }
        public required string User2Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
