using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.Entities
{
    public class Post
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(600)]
        public required string Body { get; set; }

        public string? Picture { get; set; }

        public required string UserId { get; set; }
        public User? User { get; set; }

        public List<Like> Likes { get; set; } = [];
        public List<Coments> Coments { get; set; } = [];
    }
}
