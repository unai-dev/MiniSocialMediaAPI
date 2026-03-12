namespace MiniSocialMediaAPI.DTOs.ChatMessage
{
    /// <summary>
    /// DTO que retorna los detalles completos de una conversación.
    /// Incluye la información de los participantes y todos los mensajes intercambiados.
    /// </summary>
    public class ChatDetailDTO
    {
        /// <summary>Identificador único de la conversación</summary>
        public Guid Id { get; set; }

        /// <summary>ID del primer usuario participante en el chat</summary>
        public required string UserId { get; set; }

        /// <summary>ID del segundo usuario participante en el chat</summary>
        public required string User2Id { get; set; }

        /// <summary>Lista de todos los mensajes intercambiados en la conversación</summary>
        public List<MessageDTO> Messages { get; set; } = [];
    }
}
