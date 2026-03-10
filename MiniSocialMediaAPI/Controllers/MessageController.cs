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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> Get(Guid chatId)
        {
            var chat = await context.Chats.FirstOrDefaultAsync(x => x.Id == chatId);

            if (chat is null)
            {
                return NotFound("Chat no existente");
            }

            var messages = await context.Messages
                .Where(x => x.ChatId == chatId)
                .OrderBy(o => o.CreatedAt)
                .ToListAsync();

            return Ok(mapper.Map<IEnumerable<MessageDTO>>(messages));
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> Post(Guid chatId, AddMessageDTO addMessageDTO)
        {
            var user = await userService.GetUser();

            if (user is null)
            {
                return BadRequest();
            }

            var chat = await context.Chats.FirstOrDefaultAsync(x => x.Id == chatId);

            if (chat is null)
            {
                return NotFound("Chat no existente");
            }

            if (chat.UserId != user.Id && chat.User2Id != user.Id)
            {
                return Forbid();
            }

            var message = new Message
            {
                ChatId = chatId,
                Text = addMessageDTO.Text,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            context.Add(message);

            await context.SaveChangesAsync();

            return Created();

        }


    }
}
