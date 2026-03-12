namespace MiniSocialMediaAPI.DTOs.User
{
    /// <summary>
    /// DTO para actualizar la información del perfil del usuario autenticado.
    /// Todos los campos son opcionales, permitiendo actualizar solo lo que sea necesario.
    /// </summary>
    public class UpdateUserDTO
    {
        /// <summary>Nuevo nombre de usuario (opcional)</summary>
        public string? UserName { get; set; }

        /// <summary>Nueva fecha de nacimiento (opcional)</summary>
        public DateTime? Birthdate { get; set; }

        /// <summary>Nueva URL de la foto de perfil (opcional)</summary>
        public string? Photo { get; set; }

        /// <summary>Nueva biografía del usuario (opcional)</summary>
        public string? Bio { get; set; }
    }
}
