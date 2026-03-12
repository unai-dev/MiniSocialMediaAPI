using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSocialMediaAPI.Data;
using MiniSocialMediaAPI.DTOs.ChatMessage;
using MiniSocialMediaAPI.Entities;
using MiniSocialMediaAPI.Services;

namespace MiniSocialMediaAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de chats/conversaciones entre usuarios.
    /// Permite ver las conversaciones del usuario, obtener detalles de un chat,
    /// e iniciar nuevas conversaciones con otros usuarios.
    /// Todos los endpoints requieren autenticación.
    /// </summary>
    [ApiController]
    [Route("v1/api/chats")]
    [Authorize]
    public class ChatController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public ChatController(ApplicationDbContext context, IMapper mapper, IUserService userService, UserManager<User> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userService = userService;
            this.userManager = userManager;
        }

        /// <summary>
        /// Obtiene todas las conversaciones del usuario autenticado.
        /// Retorna los chats donde el usuario es el iniciador o el participante.
        /// </summary>
        /// <returns>Lista de todos los chats del usuario actual</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatDTO>>> GetChats()
        {
            // Obtiene el usuario autenticado
            var user = await userService.GetUser();

            if (user is null)
            {
                return BadRequest();
            }

            // Obtiene todos los chats donde el usuario es participante
            var chats = await context.Chats
                .Where(x => x.UserId == user!.Id || x.User2Id == user.Id)
                .ToListAsync();

            return Ok(mapper.Map<IEnumerable<ChatDTO>>(chats));
        }

        /// <summary>
        /// Obtiene los detalles de un chat específico, incluyendo todos sus mensajes.
        /// Solo los participantes del chat pueden verlo.
        /// </summary>
        /// <param name="chatId">ID única del chat a obtener</param>
        /// <returns>Chat con todos sus mensajes incluidos</returns>
        [HttpGet("{chatId}", Name ="GetChat")]
        public async Task<ActionResult<ChatDetailDTO>> GetChat(Guid chatId)
        {
            // Busca el chat con todos sus mensajes
            var chat = await context.Chats.Include(x => x.Messages).FirstOrDefaultAsync(x => x.Id == chatId);

            if (chat is null)
            {
                return NotFound();
            }

            // Obtiene el usuario autenticado
            var user = await userService.GetUser();

            // Verifica que el usuario sea participante del chat
            if (chat.UserId != user!.Id && chat.User2Id != user.Id)
            {
                return Forbid();
            }

            return Ok(mapper.Map<ChatDetailDTO>(chat));
        }

        /// <summary>
        /// Inicia una nueva conversación con otro usuario.
        /// Si ya existe una conversación entre los usuarios, la retorna.
        /// Si no existe, crea una nueva y la retorna.
        /// </summary>
        /// <param name="userName">Nombre de usuario del participante con el que se desea chatear</param>
        /// <returns>Chat creado o existente con el otro usuario</returns>
        [HttpPost("{userName}")]
        public async Task<ActionResult> Post(string userName)
        {
            // Obtiene el usuario autenticado
            var user = await userService.GetUser();

            // Busca el usuario con el que se quiere chatear
            var userURL = await context.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            if (user is null)
            {
                return NotFound();
            }

            if (userURL is null)
            {
                return NotFound();
            }

            // Busca si ya existe una conversación entre estos dos usuarios
            var chatDB = await context.Chats
                .FirstOrDefaultAsync(u => (u.UserId == user.Id && u.User2Id == userURL.Id) ||
                (u.UserId == userURL.Id && u.User2Id == user.Id));
            
            // Si la conversación ya existe, la retorna
            if (chatDB is not null)
            {
                return Ok(mapper.Map<ChatDTO>(chatDB));
            }

            // Si no existe, crea una nueva conversación
            var chat = new Chat
            {
                UserId = user.Id,
                User2Id = userURL.Id,
                CreatedAt = DateTime.UtcNow
            };

            context.Add(chat);
            await context.SaveChangesAsync();

            var chatDTO = mapper.Map<ChatDTO>(chat);

            return CreatedAtRoute("GetChat", new {chatId = chat.Id}, chatDTO);
        }
    }
}
