namespace MiniSocialMediaAPI.DTOs.ChatMessage
{
    /// <summary>
    /// DTO que contiene la información de un mensaje individual en una conversación.
    /// Retorna el contenido del mensaje y datos del remitente.
    /// </summary>
    public class MessageDTO
    {
        /// <summary>Identificador único del mensaje</summary>
        public Guid Id { get; set; }

        /// <summary>ID del usuario que envió el mensaje</summary>
        public required string UserId { get; set; }

        /// <summary>Contenido del texto del mensaje</summary>
        public required string Text { get; set; }

        /// <summary>Fecha y hora en que se envió el mensaje</summary>
        public DateTime CreatedAt { get; set; }
    }
}
