using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.Post
{
    public class PostDTO
    {
        public Guid Id { get; set; }
        public required string Body { get; set; }
        public required string UserId { get; set; }
    }
}
