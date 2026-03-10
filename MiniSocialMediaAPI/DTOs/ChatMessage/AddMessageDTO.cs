using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.ChatMessage
{
    public class AddMessageDTO
    {

        [Required]
        [MinLength(1)]
        [MaxLength(1000)]
        public required string Text { get; set; }
    }
}
