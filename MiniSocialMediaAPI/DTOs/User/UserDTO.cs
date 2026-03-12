using MiniSocialMediaAPI.DTOs.Group;

namespace MiniSocialMediaAPI.DTOs.User
{
    /// <summary>
    /// DTO que contiene la información pública de un usuario.
    /// Se retorna cuando se solicita información de usuario, sin datos sensibles.
    /// Incluye referencias a los grupos y posts del usuario.
    /// </summary>
    public class UserDTO
    {
        /// <summary>Identificador único del usuario en el sistema</summary>
        public required string Id { get; set; }

        /// <summary>Nombre de usuario de la plataforma</summary>
        public required string UserName { get; set; }

        /// <summary>Fecha de nacimiento del usuario</summary>
        public DateTime Birthdate { get; set; }

        /// <summary>URL de la foto de perfil del usuario (opcional)</summary>
        public string? Photo { get; set; }

        /// <summary>Biografía o descripción corta del usuario (opcional)</summary>
        public string? Bio { get; set; }

        /// <summary>Lista de IDs de grupos a los que pertenece el usuario</summary>
        public List<int> GroupIds { get; set; } = [];

        /// <summary>Lista de IDs de posts publicados por el usuario</summary>
        public List<Guid> PostIds { get; set; } = [];
    }
}
