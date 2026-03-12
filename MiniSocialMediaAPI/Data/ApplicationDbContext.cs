using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniSocialMediaAPI.Entities;
using System.Reflection.Emit;

namespace MiniSocialMediaAPI.Data
{
    /// <summary>
    /// Contexto de base de datos principal de la aplicación.
    /// Hereda de IdentityDbContext<User> para gestionar usuarios con ASP.NET Identity.
    /// Define todas las tablas y relaciones entre entidades de la red social.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Tablas de la base de datos

        /// <summary>Tabla de grupos que pueden crear los usuarios</summary>
        public DbSet<Group> Groups { get; set; }

        /// <summary>Tabla de posts/publicaciones de los usuarios</summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>Tabla de likes/me gusta en los posts</summary>
        public DbSet<Like> Likes { get; set; }

        /// <summary>Tabla de comentarios en los posts</summary>
        public DbSet<Coments> Coments { get; set; }

        /// <summary>Tabla de chats/conversaciones entre usuarios</summary>
        public DbSet<Chat> Chats { get; set; }

        /// <summary>Tabla de mensajes dentro de los chats</summary>
        public DbSet<Message> Messages { get; set; }

        /// <summary>
        /// Configura las relaciones entre las entidades de la base de datos.
        /// Define comportamientos en cascada, restricciones y claves foráneas.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relación: Un post puede tener muchos likes, un like pertenece a un post
            // Cuando se elimina un post, se eliminan automáticamente sus likes (CASCADE)
            builder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación: Un usuario puede tener muchos likes, un like pertenece a un usuario
            // NoAction: no se permite eliminar usuarios que tienen likes
            builder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación: Un post puede tener muchos comentarios, un comentario pertenece a un post
            // Cuando se elimina un post, se eliminan automáticamente sus comentarios
            builder.Entity<Coments>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Coments)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación: Un usuario puede hacer muchos comentarios, un comentario pertenece a un usuario
            // NoAction: no se permite eliminar usuarios que han hecho comentarios
            builder.Entity<Coments>()
                .HasOne(l => l.User)
                .WithMany(u => u.Coments)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación: Un chat pertenece a un usuario (usuario 1)
            // Restrict: no se permite eliminar usuarios que tienen chats activos
            builder.Entity<Chat>()
                .HasOne(c => c.User)
                .WithMany()             
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación: Un chat pertenece a otro usuario (usuario 2)
            // Restrict: no se permite eliminar usuarios que tienen chats activos
            builder.Entity<Chat>()
                .HasOne(c => c.User2)
                .WithMany()
                .HasForeignKey(c => c.User2Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación: Un chat puede tener muchos mensajes, un mensaje pertenece a un chat
            // Cuando se elimina un chat, se eliminan automáticamente sus mensajes
            builder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
