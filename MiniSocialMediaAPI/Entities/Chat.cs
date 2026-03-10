namespace MiniSocialMediaAPI.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }

        public required string UserId { get; set; }
        public User? User { get; set; }

        public required string User2Id { get; set; }
        public User? User2 { get; set; }

        public List<Message> Messages { get; set; } = [];

        public DateTime CreatedAt { get; set; }
    }
}
