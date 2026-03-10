using MiniSocialMediaAPI.DTOs.Group;

namespace MiniSocialMediaAPI.DTOs.User
{
    public class UserDTO
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public DateTime Birthdate { get; set; }
        public string? Photo { get; set; }
        public string? Bio { get; set; }


        public List<int> GroupIds { get; set; } = [];
        public List<Guid> PostIds { get; set; } = [];
    }
}
