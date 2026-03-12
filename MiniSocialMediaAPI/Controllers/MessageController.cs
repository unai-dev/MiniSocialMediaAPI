using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSocialMediaAPI.Data;
using MiniSocialMediaAPI.DTOs.ChatMessage;
using MiniSocialMediaAPI.Entities;
using MiniSocialMediaAPI.Services;

namespace MiniSocialMediaAPI.Controllers
{
    /// <summary>
    /// Controlador para gestionar mensajes dentro de los chats.
    /// Permite ver el historial de mensajes y enviar nuevos mensajes en una conversación.
    /// La ruta incluye el ID del chat: /api/chats/{chatId}/messages
    /// Solo los participantes del chat pueden acceder a estos endpoints.
    /// </summary>
    [ApiController]
    [Route("v1/api/chats/{chatId}/messages")]
    [Authorize]
    public class MessageController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public MessageController(ApplicationDbContext context, IMapper mapper, IUserService userService)
        {
            this.context = context;
            this.mapper = mapper;
            this.userService = userService;
        }

        /// <summary>
        /// Obtiene todos los mensajes de un chat específico.
        /// Los mensajes se retornan en orden cronológico (del más antiguo al más reciente).
        /// </summary>
        /// <param name="chatId">ID del chat del que se desean obtener los mensajes</param>
        /// <returns>Lista de mensajes del chat ordenados por fecha de creación</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> Get(Guid chatId)
        {
            // Verifica que el chat exista
            var chat = await context.Chats.AnyAsync(x => x.Id == chatId);

            if (!chat)
            {
                return NotFound("Chat no existente");
            }

            // Obtiene todos los mensajes del chat ordenados por fecha de creación
            var messages = await context.Messages
                .Where(x => x.ChatId == chatId)
                .OrderBy(o => o.CreatedAt)
                .ToListAsync();

            return Ok(mapper.Map<IEnumerable<MessageDTO>>(messages));
        }

        /// <summary>
        /// Envía un nuevo mensaje en el chat.
        /// El mensaje se asocia automáticamente al usuario autenticado y la hora actual.
        /// Solo los participantes del chat pueden enviar mensajes.
        /// </summary>
        /// <param name="chatId">ID del chat donde se desea enviar el mensaje</param>
        /// <param name="addMessageDTO">Contenido del mensaje a enviar</param>
        /// <returns>201 Created si el mensaje se envía exitosamente</returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> Post(Guid chatId, AddMessageDTO addMessageDTO)
        {
            // Obtiene el usuario autenticado
            var user = await userService.GetUser();

            if (user is null)
            {
                return BadRequest();
            }

            // Busca el chat para verificar que existe y obtener su información
            var chat = await context.Chats.FirstOrDefaultAsync(x => x.Id == chatId);

            if (chat is null)
            {
                return NotFound("Chat no existente");
            }

            // Verifica que el usuario sea participante del chat
            if (chat.UserId != user.Id && chat.User2Id != user.Id)
            {
                return Forbid();
            }

            // Crea el nuevo mensaje con la información del usuario y la hora actual
            var message = new Message
            {
                ChatId = chatId,
                Text = addMessageDTO.Text,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            // Guarda el mensaje en la base de datos
            context.Add(message);
            await context.SaveChangesAsync();

            return Created();
        }
    }
}
