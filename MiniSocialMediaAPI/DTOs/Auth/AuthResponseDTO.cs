namespace MiniSocialMediaAPI.DTOs.Auth
{
    /// <summary>
    /// DTO que contiene la respuesta del servidor después de un login o registro exitoso.
    /// Retorna el token JWT que el cliente debe usar para autenticarse en futuras solicitudes.
    /// </summary>
    public class AuthResponseDTO
    {
        /// <summary>Token JWT codificado que se debe enviar en el header de autorización</summary>
        public required string Token { get; set; }

        /// <summary>Fecha y hora en la que el token expirará y ya no será válido</summary>
        public DateTime Expires { get; set; }
    }
}
