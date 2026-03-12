using MiniSocialMediaAPI.DTOs.User;

namespace MiniSocialMediaAPI.DTOs.Group
{
    /// <summary>
    /// DTO que contiene la información de un grupo.
    /// Retorna los detalles del grupo incluyendo sus miembros.
    /// </summary>
    public class GroupDTO
    {
        /// <summary>Identificador único del grupo</summary>
        public int Id { get; set; }

        /// <summary>Nombre del grupo</summary>
        public required string Name { get; set; }

        /// <summary>Lista de IDs de usuarios que son miembros del grupo</summary>
        public List<string> UserIds { get; set; } = [];

        /// <summary>Número máximo de miembros permitidos en el grupo</summary>
        public int MaxMembers { get; set; }
    }
}
