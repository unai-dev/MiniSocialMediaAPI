using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.Entities
{
    /// <summary>
    /// Entidad que representa una publicación o post creado por un usuario.
    /// Contiene el contenido de texto y opcionalmente una imagen.
    /// Los posts pueden recibir "me gusta" y comentarios de otros usuarios.
    /// </summary>
    public class Post
    {
        /// <summary>Identificador único del post</summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Contenido principal del post.
        /// Debe tener entre 2 y 600 caracteres.
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(600)]
        public required string Body { get; set; }

        /// <summary>URL de una imagen adjunta al post (opcional)</summary>
        public string? Picture { get; set; }

        // Relaciones con otras entidades

        /// <summary>ID del usuario que creó este post</summary>
        public required string UserId { get; set; }

        /// <summary>Referencia a la entidad User que creó el post</summary>
        public User? User { get; set; }

        /// <summary>Lista de "me gusta" que ha recibido este post</summary>
        public List<Like> Likes { get; set; } = [];

        /// <summary>Lista de comentarios que ha recibido este post</summary>
        public List<Coments> Coments { get; set; } = [];
    }
}
