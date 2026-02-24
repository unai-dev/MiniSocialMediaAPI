using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.User
{
    public class UserCredentialsDTO
    {
        [Required]
        [MinLength(5)]
        public required string UserName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
