using Microsoft.AspNetCore.Identity;

namespace MiniSocialMediaAPI.Entities
{
    /// <summary>
    /// Entidad que representa un usuario registrado en la aplicación.
    /// Hereda de IdentityUser para aprovechar la gestión integrada de autenticación de ASP.NET Identity.
    /// Cada usuario puede crear posts, comentarios, likes, chats y unirse a grupos.
    /// </summary>
    public class User: IdentityUser
    {
        /// <summary>Fecha de nacimiento del usuario</summary>
        public DateTime Birthdate { get; set; }

        /// <summary>URL de la foto de perfil del usuario</summary>
        public string? Photo { get; set; }

        /// <summary>Biografía o descripción corta del usuario</summary>
        public string? Bio { get; set; }

        // Relaciones con otras entidades

        /// <summary>Lista de grupos a los que pertenece este usuario</summary>
        public List<Group> Groups { get; set; } = [];

        /// <summary>Lista de publicaciones creadas por este usuario</summary>
        public List<Post> Posts { get; set; } = [];

        /// <summary>Lista de "me gusta" que ha dado este usuario a posts</summary>
        public List<Like> Likes { get; set; } = [];

        /// <summary>Lista de comentarios que ha hecho este usuario en posts</summary>
        public List<Coments> Coments { get; set; } = [];

        /// <summary>Lista de chats iniciados por este usuario</summary>
        public List<Chat> Chats { get; set; } = [];

        /// <summary>Lista de mensajes enviados por este usuario en los chats</summary>
        public List<Message> Messages { get; set; } = [];
    }
}
