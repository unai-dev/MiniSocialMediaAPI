using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.Coment
{
    public class AddComentDTO
    {
        [Required]
        [MinLength(2)]
        [MaxLength(400)]
        public required string Body { get; set; }
    }
}
