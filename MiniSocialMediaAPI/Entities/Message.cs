using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.Entities
{
    public class Message
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(1000)]
        public required string Text { get; set; }

        public required string UserId { get; set; }
        public User? User { get; set; }

        public Guid ChatId { get; set; }
        public Chat? Chat { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
