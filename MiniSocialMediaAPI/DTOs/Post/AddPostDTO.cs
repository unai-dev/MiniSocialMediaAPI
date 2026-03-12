using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.Post
{
    /// <summary>
    /// DTO para crear un nuevo post/publicación.
    /// El usuario autenticado es asignado automáticamente como autor del post.
    /// </summary>
    public class AddPostDTO
    {
        /// <summary>
        /// Contenido principal del post.
        /// Debe tener entre 2 y 600 caracteres.
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(600)]
        public required string Body { get; set; }
    }
}
