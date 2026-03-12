using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.Entities
{
    /// <summary>
    /// Entidad que representa un comentario en un post.
    /// Los usuarios pueden comentar en los posts de otros usuarios para iniciar conversaciones.
    /// Cada comentario debe tener entre 2 y 400 caracteres.
    /// </summary>
    public class Coments
    {
        /// <summary>Identificador único del comentario</summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Contenido del comentario.
        /// Debe tener entre 2 y 400 caracteres.
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(400)]
        public required string Body { get; set; }

        // Relaciones con otras entidades

        /// <summary>ID del post en el que se hizo el comentario</summary>
        public Guid PostId { get; set; }

        /// <summary>Referencia a la entidad Post comentado</summary>
        public Post? Post { get; set; }

        /// <summary>ID del usuario que escribió el comentario</summary>
        public required string UserId { get; set; }

        /// <summary>Referencia a la entidad User que escribió el comentario</summary>
        public User? User { get; set; }
    }
}
