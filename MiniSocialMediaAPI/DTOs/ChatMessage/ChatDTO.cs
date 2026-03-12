using MiniSocialMediaAPI.Entities;

namespace MiniSocialMediaAPI.DTOs.ChatMessage
{
    /// <summary>
    /// DTO que retorna información básica de una conversación entre dos usuarios.
    /// Contiene solo los IDs de los participantes, sin incluir los mensajes.
    /// </summary>
    public class ChatDTO
    {
        /// <summary>Identificador único de la conversación</summary>
        public Guid Id { get; set; }
        
        /// <summary>ID del primer usuario participante en el chat</summary>
        public required string UserId { get; set; }

        /// <summary>ID del segundo usuario participante en el chat</summary>
        public required string User2Id { get; set; }
    }
}
