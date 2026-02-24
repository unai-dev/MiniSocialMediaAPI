using MiniSocialMediaAPI.DTOs.User;

namespace MiniSocialMediaAPI.DTOs.Group
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<string> UserIds { get; set; } = [];
        public int MaxMembers { get; set; }
    }
}
