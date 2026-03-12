using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.Group
{
    /// <summary>
    /// DTO para crear un nuevo grupo en la aplicación.
    /// Contiene la información básica necesaria para crear un grupo.
    /// </summary>
    public class AddGroupDTO
    {
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
        /// Por defecto es 200. Puede ser modificado al crear el grupo.
        /// </summary>
        public int? MaxMembers { get; set; } = 200;
    }
}
