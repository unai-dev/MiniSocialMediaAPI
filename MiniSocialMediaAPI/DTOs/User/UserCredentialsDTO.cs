using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.User
{
    /// <summary>
    /// DTO que recibe las credenciales del usuario para login o registro.
    /// Contiene el nombre de usuario, email y contraseña requeridos para autenticarse.
    /// </summary>
    public class UserCredentialsDTO
    {
        /// <summary>
        /// Nombre de usuario único en la plataforma.
        /// Debe tener al menos 5 caracteres.
        /// </summary>
        [Required]
        [MinLength(5)]
        public required string UserName { get; set; }

        /// <summary>
        /// Email del usuario en formato válido.
        /// Se utiliza como identificador principal para buscar al usuario.
        /// </summary>
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// Contraseña del usuario en texto plano.
        /// Se encripta antes de ser almacenada en la base de datos.
        /// </summary>
        [Required]
        public string? Password { get; set; }
    }
}
