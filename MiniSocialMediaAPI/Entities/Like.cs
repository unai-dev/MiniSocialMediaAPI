namespace MiniSocialMediaAPI.Entities
{
    /// <summary>
    /// Entidad que representa un "me gusta" en un post.
    /// Establece la relación entre un usuario y el post al que le dio me gusta.
    /// Cada usuario solo puede dar un like por post (evitado a nivel de lógica).
    /// </summary>
    public class Like
    {
        /// <summary>Identificador único del like</summary>
        public Guid Id { get; set; }

        // Relaciones con otras entidades

        /// <summary>ID del usuario que dio el like</summary>
        public required string UserId { get; set; }

        /// <summary>Referencia a la entidad User que dio el like</summary>
        public User? User { get; set; }

        /// <summary>ID del post que recibió el like</summary>
        public Guid PostId { get; set; }

        /// <summary>Referencia a la entidad Post que recibió el like</summary>
        public Post? Post { get; set; }
    }
}
