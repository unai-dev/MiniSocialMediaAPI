using MiniSocialMediaAPI.DTOs.User;

namespace MiniSocialMediaAPI.DTOs.Group
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<UserDTO> Users { get; set; } = [];
    }
}
