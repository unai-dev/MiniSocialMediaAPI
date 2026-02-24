using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.Auth
{
    public class EditClaimDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
