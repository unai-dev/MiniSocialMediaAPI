using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.Entities
{
    /// <summary>
    /// Entidad que representa un mensaje individual en una conversación privada.
    /// Cada mensaje pertenece a un chat específico y es enviado por un usuario.
    /// Los mensajes deben tener entre 1 y 1000 caracteres.
    /// </summary>
    public class Message
    {
        /// <summary>Identificador único del mensaje</summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Contenido del mensaje.
        /// Debe tener entre 1 y 1000 caracteres.
        /// </summary>
        [Required]
        [MinLength(1)]
        [MaxLength(1000)]
        public required string Text { get; set; }

        // Relaciones con otras entidades

        /// <summary>ID del usuario que envió el mensaje</summary>
        public required string UserId { get; set; }

        /// <summary>Referencia a la entidad User que envió el mensaje</summary>
        public User? User { get; set; }

        /// <summary>ID de la conversación a la que pertenece el mensaje</summary>
        public Guid ChatId { get; set; }

        /// <summary>Referencia a la entidad Chat que contiene el mensaje</summary>
        public Chat? Chat { get; set; }

        /// <summary>Fecha y hora en que se envió el mensaje</summary>
        public DateTime CreatedAt { get; set; }
    }
}
