using MiniSocialMediaAPI.Entities;

namespace MiniSocialMediaAPI.Services
{
    public interface IUserService
    {
        Task<User?> GetUser();
    }
}