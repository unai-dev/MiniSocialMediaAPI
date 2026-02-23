using MiniSocialMediaAPI.DTOs.Group;

namespace MiniSocialMediaAPI.DTOs.User
{
    public class UserDTO
    {
        public required string UserName { get; set; }
        public DateTime Birthdate { get; set; }
        public List<GroupDTO> Groups { get; set; } = [];
    }
}
