using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.Group
{
    public class AddGroupDTO
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public required string Name { get; set; }
        public int? MaxMembers { get; set; } = 200;
    }
}
