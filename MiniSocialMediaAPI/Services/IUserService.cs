using MiniSocialMediaAPI.Entities;

namespace MiniSocialMediaAPI.Services
{
    /**
     * INTERFACE
     * Interfaz para no depender directamente del servicio de usuarios
     */
    public interface IUserService
    {
        Task<User?> GetUser();
    }
}