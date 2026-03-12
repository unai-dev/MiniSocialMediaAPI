using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.Coment
{
    /// <summary>
    /// DTO para crear un nuevo comentario en un post.
    /// El usuario autenticado es asignado automáticamente como autor del comentario.
    /// </summary>
    public class AddComentDTO
    {
        /// <summary>
        /// Contenido del comentario.
        /// Debe tener entre 2 y 400 caracteres.
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(400)]
        public required string Body { get; set; }
    }
}
