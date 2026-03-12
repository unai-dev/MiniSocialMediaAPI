using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSocialMediaAPI.Data;
using MiniSocialMediaAPI.Entities;
using MiniSocialMediaAPI.Services;

namespace MiniSocialMediaAPI.Controllers
{
    /// <summary>
    /// Controlador para gestionar los "me gusta" en los posts.
    /// Permite agregar y eliminar likes de los posts.
    /// La ruta incluye el ID del post en la URL: /api/posts/{postId}/likes
    /// Todos los endpoints requieren autenticación.
    /// </summary>
    [ApiController]
    [Route("v1/api/posts/{postId}/likes")]
    [Authorize]
    public class LikeController: ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;

        public LikeController(IUserService userService, IMapper mapper, ApplicationDbContext context)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.context = context;
        }

        /// <summary>
        /// Agrega un "me gusta" del usuario autenticado al post especificado.
        /// El mismo usuario no puede dar like múltiples veces al mismo post.
        /// </summary>
        /// <param name="postId">ID del post al que se quiere dar like</param>
        /// <returns>200 OK si se agrega exitosamente, 400 si el like ya existe</returns>
        [HttpPost]
        public async Task<ActionResult> Post(Guid postId)
        {
            // Obtiene el usuario autenticado
            var user = await userService.GetUser();

            // Verifica que el post exista
            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == postId);

            if (post is null)
            {
                return NotFound();
            }

            // Verifica si el usuario ya ha dado like a este post
            var likeExists = await context.Likes.AnyAsync(u => u.UserId == user!.Id && u.PostId == postId);

            if (likeExists)
            {
                return BadRequest("El like ya existe");
            }

            // Crea nuevo like del usuario al post
            var like = new Like
            {
                PostId = post.Id,
                UserId = user!.Id
            };

            // Guarda el like en la base de datos
            context.Likes.Add(like);
            await context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Elimina un "me gusta" del usuario autenticado en un post.
        /// El usuario solo puede eliminar sus propios likes.
        /// </summary>
        /// <param name="postId">ID del post del que se quiere remover el like</param>
        /// <param name="id">ID del like a eliminar</param>
        /// <returns>204 No Content si se elimina exitosamente</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid postId, Guid id)
        {
            // Obtiene el usuario autenticado
            var user = await userService.GetUser();

            if (user is null)
            {
                return BadRequest();
            }

            // Verifica que el post exista
            var post = await context.Posts.AnyAsync(x => x.Id == postId);

            if (!post)
            {
                return NotFound();
            }

            // Elimina el like solo si pertenece al usuario autenticado
            var like = await context.Likes.Where(l => l.Id == id && l.UserId == user.Id).ExecuteDeleteAsync();

            if (like == 0)
            {
                return BadRequest("Like inexistente/No autorizado");
            }

            return NoContent();
        }
    }
}
