using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSocialMediaAPI.Data;
using MiniSocialMediaAPI.DTOs.Coment;
using MiniSocialMediaAPI.Entities;
using MiniSocialMediaAPI.Services;

namespace MiniSocialMediaAPI.Controllers
{
    /// <summary>
    /// Controlador para gestionar comentarios en los posts.
    /// Permite ver, crear y eliminar comentarios en publicaciones.
    /// La ruta incluye el ID del post: /api/posts/{postId}/coments
    /// Todos los endpoints requieren autenticación.
    /// </summary>
    [ApiController]
    [Route("v1/api/posts/{postId}/coments")]
    [Authorize]
    public class ComentController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public ComentController(ApplicationDbContext context, IMapper mapper, IUserService userService)
        {
            this.context = context;
            this.mapper = mapper;
            this.userService = userService;
        }

        /// <summary>
        /// Obtiene todos los comentarios de un post específico.
        /// Retorna los comentarios ordenados e incluye información del autor.
        /// </summary>
        /// <param name="postId">ID del post del que se desean obtener los comentarios</param>
        /// <returns>Lista de comentarios del post solicitado</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComentDTO>>> Get(Guid postId)
        {
            // Verifica que el post exista
            var post = await context.Posts.AnyAsync(x => x.Id == postId);

            if (!post)
            {
                return NotFound();
            }

            // Obtiene todos los comentarios del post incluyendo información del usuario autor
            var coments = await context.Coments
                .Where(x => x.PostId == postId)
                .Include(u => u.User)
                .ToListAsync();

            return Ok(mapper.Map<IEnumerable<ComentDTO>>(coments));
        }

        /// <summary>
        /// Crea un nuevo comentario en un post.
        /// El comentario se asocia automáticamente al usuario autenticado.
        /// </summary>
        /// <param name="postId">ID del post en el que se desea comentar</param>
        /// <param name="addComentDTO">Contenido del comentario</param>
        /// <returns>El comentario creado con su información</returns>
        [HttpPost]
        public async Task<ActionResult> Post(Guid postId, AddComentDTO addComentDTO)
        {
            // Verifica que el post exista
            var post = await context.Posts.AnyAsync(x => x.Id == postId);

            if (!post)
            {
                return NotFound();
            }

            // Obtiene el usuario autenticado
            var user = await userService.GetUser();

            if (user is null)
            {
                return BadRequest();
            }

            // Mapea el DTO a la entidad Comentario
            var coment = mapper.Map<Coments>(addComentDTO);
            
            // Asigna el post y el usuario al comentario
            coment.PostId = postId;
            coment.UserId = user.Id;

            // Guarda el comentario en la base de datos
            context.Add(coment);
            await context.SaveChangesAsync();

            return Ok(mapper.Map<ComentDTO>(coment));
        }

        /// <summary>
        /// Elimina un comentario del usuario autenticado.
        /// Solo el autor del comentario puede eliminarlo.
        /// </summary>
        /// <param name="postId">ID del post que contiene el comentario</param>
        /// <param name="id">ID del comentario a eliminar</param>
        /// <returns>204 No Content si se elimina exitosamente</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid postId, Guid id)
        {
            // Obtiene el usuario autenticado
            var user = await userService.GetUser();

            // Verifica que el post exista
            var post = await context.Posts.AnyAsync(x => x.Id == postId);

            if (!post)
            {
                return NotFound();
            }

            // Elimina el comentario solo si pertenece al usuario autenticado
            var coment = await context.Coments.Where(x => x.PostId == postId && x.UserId == user.Id).ExecuteDeleteAsync();

            if (coment == 0)
            {
                return Forbid("No estás autorizado a borrar el comentario");
            }

            return NoContent();
        }
    }
}
