using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.Entities
{
    public class Group
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public required string Name { get; set; }
        public int? MaxMembers { get; set; } = 200;
        public List<User> Users { get; set; } = [];
    }
}
