using Microsoft.AspNetCore.Identity;

namespace MiniSocialMediaAPI.Entities
{
    public class User: IdentityUser
    {
        public DateTime Birthdate { get; set; }
        public List<Group> Groups { get; set; } = [];
    }
}
