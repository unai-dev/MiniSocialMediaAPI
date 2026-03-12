using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.Auth
{
    /// <summary>
    /// DTO para asignar permisos o roles a un usuario existente.
    /// Se utiliza en el endpoint para convertir a un usuario en administrador.
    /// </summary>
    public class EditClaimDTO
    {
        /// <summary>Email del usuario al que se le desea asignar o modificar permisos</summary>
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
