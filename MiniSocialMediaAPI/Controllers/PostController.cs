using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSocialMediaAPI.Data;
using MiniSocialMediaAPI.DTOs.Post;
using MiniSocialMediaAPI.Entities;
using MiniSocialMediaAPI.Services;

namespace MiniSocialMediaAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de posts/publicaciones.
    /// Permite crear, leer y eliminar publicaciones en la red social.
    /// Todos los endpoints requieren autenticación con un token JWT válido.
    /// </summary>
    [ApiController]
    [Route("v1/api/posts")]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public PostController(ApplicationDbContext context, IMapper mapper, IUserService userService)
        {
            this.context = context;
            this.mapper = mapper;
            this.userService = userService;
        }

        /// <summary>
        /// Obtiene todos los posts de la aplicación.
        /// Incluye información de likes y comentarios para cada post.
        /// </summary>
        /// <returns>Lista de todos los posts con sus likes y comentarios</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> Get()
        {
            // Obtiene todos los posts con sus relaciones (likes y comentarios)
            var posts = await context.Posts
                .Include(l => l.Likes)
                .Include(u => u.UserId)
                .Include(c => c.Coments)
                .ToListAsync();

            return Ok(mapper.Map<IEnumerable<PostDTO>>(posts));
        }

        /// <summary>
        /// Obtiene un post específico por su ID.
        /// Incluye toda la información de likes y comentarios del post.
        /// </summary>
        /// <param name="id">ID único del post a obtener</param>
        /// <returns>Datos del post solicitado o 404 si no existe</returns>
        [HttpGet("{id}", Name ="GetPost")]
        public async Task<ActionResult<PostDTO>> Get(Guid id)
        {
            // Busca el post por ID con toda su información relacionada
            var post = await context.Posts
                .Include(l => l.Likes)
                .Include(u => u.UserId)
                .Include(c => c.Coments)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (post is null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<PostDTO>(post));
        }

        /// <summary>
        /// Crea un nuevo post para el usuario autenticado.
        /// El post se asocia automáticamente al usuario que lo publica.
        /// </summary>
        /// <param name="addPostDTO">Información del post: contenido (body) e imagen opcional</param>
        /// <returns>Datos del post creado con su ID</returns>
        [HttpPost]
        public async Task<ActionResult> Post(AddPostDTO addPostDTO)
        {
            // Obtiene el usuario autenticado actual
            var user = await userService.GetUser();
            if (user is null)
            {
                return BadRequest();
            }

            // Mapea el DTO a la entidad Post
            var post = mapper.Map<Post>(addPostDTO);
            
            // Asigna el usuario actual como autor del post
            post.UserId = user.Id;

            // Guarda el post en la base de datos
            context.Add(post);
            await context.SaveChangesAsync();

            // Mapea el post creado a DTO para la respuesta
            var postDTO = mapper.Map<PostDTO>(post);

            // Retorna el post creado con su ubicación (para poder acceder a él después)
            return CreatedAtRoute("GetPost", new { id = post.Id }, postDTO);
        }

        /// <summary>
        /// Elimina un post existente.
        /// Solo el autor del post puede eliminarlo.
        /// </summary>
        /// <param name="id">ID del post a eliminar</param>
        /// <returns>204 No Content si se elimina correctamente</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            // Obtiene el usuario autenticado
            var user = await userService.GetUser();

            if (user is null)
            {
                return BadRequest();
            }

            // Busca el post a eliminar
            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == id);

            // Verifica que el usuario sea el autor del post
            if (post!.UserId != user.Id)
            {
                return Forbid();
            }

            // Elimina el post de la base de datos
            context.Remove(post);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
