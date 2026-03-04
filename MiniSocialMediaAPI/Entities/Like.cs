namespace MiniSocialMediaAPI.Entities
{
    public class Like
    {
        public Guid Id { get; set; }

        public required string UserId { get; set; }
        public User? User { get; set; }
        public Guid PostId { get; set; }
        public Post? Post { get; set; }
    }
}
