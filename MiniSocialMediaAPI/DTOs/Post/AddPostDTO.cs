using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.Post
{
    public class AddPostDTO
    {
        [Required]
        [MinLength(2)]
        [MaxLength(600)]
        public required string Body { get; set; }
    }
}
