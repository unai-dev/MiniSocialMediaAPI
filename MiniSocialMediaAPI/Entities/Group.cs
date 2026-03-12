using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.Entities
{
    /// <summary>
    /// Entidad que representa un grupo en la aplicación.
    /// Los usuarios pueden crear y unirse a grupos para compartir intereses comunes.
    /// Cada grupo tiene un nombre requerido y un límite máximo de miembros (por defecto 200).
    /// </summary>
    public class Group
    {
        /// <summary>Identificador único del grupo</summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre del grupo.
        /// Debe tener entre 5 y 100 caracteres.
        /// </summary>
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public required string Name { get; set; }

        /// <summary>
        /// Número máximo de miembros permitidos en el grupo.
        /// Por defecto es 200 miembros.
        /// </summary>
        public int? MaxMembers { get; set; } = 200;

        /// <summary>Lista de todos los usuarios que pertenecen a este grupo</summary>
        public List<User> Users { get; set; } = [];
    }
}
