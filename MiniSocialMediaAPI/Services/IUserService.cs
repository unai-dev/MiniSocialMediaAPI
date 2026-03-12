using MiniSocialMediaAPI.Entities;

namespace MiniSocialMediaAPI.Services
{
    /// <summary>
    /// Interfaz que define el contrato para el servicio de usuarios.
    /// Esta interfaz se utiliza para desacoplar la implementación del servicio
    /// de su uso en los controladores, permitiendo mejor testabilidad.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Obtiene la información del usuario autenticado actualmente.
        /// Extrae el email del token JWT y busca el usuario en la base de datos.
        /// </summary>
        /// <returns>
        /// Retorna la entidad User del usuario autenticado, o null si no se encuentra.
        /// </returns>
        Task<User?> GetUser();
    }
}