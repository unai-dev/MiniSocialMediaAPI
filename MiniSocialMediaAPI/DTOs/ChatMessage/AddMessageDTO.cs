using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.ChatMessage
{
    public class AddMessageDTO
    {
        public required string UserId { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(1000)]
        public required string Text { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
