using Microsoft.AspNetCore.Identity;
using MiniSocialMediaAPI.Entities;

namespace MiniSocialMediaAPI.Services
{
    /// <summary>
    /// Implementación del servicio de usuarios.
    /// Implementa la interfaz IUserService para desacoplar el código del acceso de datos.
    /// Se utiliza para obtener información del usuario autenticado a partir del contexto HTTP.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor contextAccessor;

        public UserService(UserManager<User> userManager, IHttpContextAccessor contextAccessor)
        {
            this.userManager = userManager;
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Obtiene el usuario autenticado desde el contexto HTTP actual.
        /// Extrae el email del claim de autorización del token JWT
        /// y lo utiliza para buscar el usuario en la base de datos.
        /// </summary>
        /// <returns>
        /// La entidad User del usuario autenticado, o null si no hay usuario autenticado.
        /// </returns>
        public async Task<User?> GetUser()
        {
            // Extrae el claim de email del contexto actual
            var claimEmail = contextAccessor.HttpContext!
                .User.Claims.Where(x => x.Type == "email").FirstOrDefault();

            // Si no hay claim de email, retorna null
            if (claimEmail is null)
            {
                return null;
            }

            // Obtiene el valor del email y busca el usuario en la base de datos
            var email = claimEmail.Value;
            return await userManager.FindByEmailAsync(email);
        }
    }
}
