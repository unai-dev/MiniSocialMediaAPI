using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.Coment
{
    /// <summary>
    /// DTO que contiene la información de un comentario en un post.
    /// Retorna el contenido del comentario y datos del autor.
    /// </summary>
    public class ComentDTO
    {
        /// <summary>Identificador único del comentario</summary>
        public Guid Id { get; set; }

        /// <summary>Contenido del comentario</summary>
        public required string Body { get; set; }

        /// <summary>ID del post que contiene el comentario</summary>
        public Guid PostId { get; set; }

        /// <summary>ID del usuario que escribió el comentario</summary>
        public required string UserId { get; set; }
    }
}
