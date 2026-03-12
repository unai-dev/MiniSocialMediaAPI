using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.ChatMessage
{
    /// <summary>
    /// DTO para enviar un nuevo mensaje en una conversación.
    /// El usuario autenticado y la hora actual se asignan automáticamente.
    /// </summary>
    public class AddMessageDTO
    {
        /// <summary>
        /// Contenido del mensaje a enviar.
        /// Debe tener entre 1 y 1000 caracteres.
        /// </summary>
        [Required]
        [MinLength(1)]
        [MaxLength(1000)]
        public required string Text { get; set; }
    }
}
