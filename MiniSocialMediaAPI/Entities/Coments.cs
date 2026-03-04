using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.Entities
{
    public class Coments
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(400)]
        public required string Body { get; set; }

        public Guid PostId { get; set; }
        public Post? Post { get; set; }
        public required string UserId { get; set; }
        public User? User { get; set; }
    }
}
