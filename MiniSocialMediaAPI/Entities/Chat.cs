namespace MiniSocialMediaAPI.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }
        public List<Message> Messages { get; set; } = [];
        public List<User> Users { get; set; } = [];
    }
}
