using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.User
{
    public class UserCredentialsDTO
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
