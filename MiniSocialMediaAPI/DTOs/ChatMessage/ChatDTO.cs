using MiniSocialMediaAPI.Entities;

namespace MiniSocialMediaAPI.DTOs.ChatMessage
{
    public class ChatDTO
    {
        public Guid Id { get; set; }
        public List<Message> Messages { get; set; } = [];
        public List<string> UsersIds { get; set; } = [];
    }
}
