using MiniSocialMediaAPI.DTOs.Coment;
using MiniSocialMediaAPI.DTOs.Like;
using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.Post
{
    /// <summary>
    /// DTO que retorna la información completa de un post.
    /// Incluye el contenido, autor, lista de likes y comentarios.
    /// </summary>
    public class PostDTO
    {
        /// <summary>Identificador único del post</summary>
        public Guid Id { get; set; }

        /// <summary>Contenido del post(texto principal)</summary>
        public required string Body { get; set; }

        /// <summary>ID del usuario que escribió el post</summary>
        public required string UserId { get; set; }

        /// <summary>Lista de DTOs de likes que ha recibido el post</summary>
        public List<LikeDTO> Likes { get; set; } = [];

        /// <summary>Lista de DTOs de comentarios que ha recibido el post</summary>
        public List<ComentDTO> Coments { get; set; } = [];
    }
}
