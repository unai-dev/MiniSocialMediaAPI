namespace MiniSocialMediaAPI.DTOs.Like
{
    /// <summary>
    /// DTO que contiene la información de un "me gusta" en un post.
    /// Retorna los datos básicos del like incluyendo usuario y post relacionados.
    /// </summary>
    public class LikeDTO
    {
        /// <summary>Identificador único del like</summary>
        public Guid Id { get; set; }

        /// <summary>ID del usuario que dio el like</summary>
        public required string UserId { get; set; }

        /// <summary>ID del post que recibió el like</summary>
        public Guid PostId { get; set; }
    }
}
