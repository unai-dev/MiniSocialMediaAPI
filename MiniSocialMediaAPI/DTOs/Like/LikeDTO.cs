namespace MiniSocialMediaAPI.DTOs.Like
{
    public class LikeDTO
    {
        public Guid Id { get; set; }
        public required string UserId { get; set; }
        public int PostId { get; set; }
    }
}
