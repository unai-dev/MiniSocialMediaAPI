namespace MiniSocialMediaAPI.DTOs.ChatMessage
{
    public class ChatDetailDTO
    {
        public Guid Id { get; set; }

        public required string UserId { get; set; }
        public required string User2Id { get; set; }
        public List<MessageDTO> Messages { get; set; } = [];
    }
}
