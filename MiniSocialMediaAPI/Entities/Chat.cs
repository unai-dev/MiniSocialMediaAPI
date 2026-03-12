namespace MiniSocialMediaAPI.Entities
{
    /// <summary>
    /// Entidad que representa una conversación privada entre dos usuarios.
    /// Una conversación es bilateral, es decir, ambos usuarios pueden enviar y recibir mensajes.
    /// Los mensajes del chat se almacenan en la entidad Message relacionada.
    /// </summary>
    public class Chat
    {
        /// <summary>Identificador único de la conversación</summary>
        public Guid Id { get; set; }

        // Relaciones con las entidades User (participantes del chat)

        /// <summary>ID del primer usuario participante en la conversación</summary>
        public required string UserId { get; set; }

        /// <summary>Referencia a la entidad User del primer participante</summary>
        public User? User { get; set; }

        /// <summary>ID del segundo usuario participante en la conversación</summary>
        public required string User2Id { get; set; }

        /// <summary>Referencia a la entidad User del segundo participante</summary>
        public User? User2 { get; set; }

        /// <summary>Lista de todos los mensajes intercambiados en esta conversación</summary>
        public List<Message> Messages { get; set; } = [];

        /// <summary>Fecha y hora en la que se creó esta conversación</summary>
        public DateTime CreatedAt { get; set; }
    }
}
