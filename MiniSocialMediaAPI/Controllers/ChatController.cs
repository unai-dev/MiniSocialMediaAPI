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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatDTO>>> GetChats()
        {
            var user = await userService.GetUser();

            if (user is null)
            {
                return BadRequest();
            }

            var chats = await context.Chats.Where(x => x.UserId == user!.Id || x.User2Id == user.Id).ToListAsync();

            return Ok(mapper.Map<IEnumerable<ChatDTO>>(chats));
        }

        [HttpGet("{chatId}", Name ="GetChat")]
        public async Task<ActionResult<ChatDetailDTO>> GetChat(Guid chatId)
        {
            var chat = await context.Chats.Include(x => x.Messages).FirstOrDefaultAsync(x => x.Id == chatId);

            if (chat is null)
            {
                return NotFound();
            }

            var user = await userService.GetUser();

            if (chat.UserId != user!.Id && chat.User2Id != user.Id)
            {
                return Forbid();
            }

            return Ok(mapper.Map<ChatDetailDTO>(chat));
        }

        [HttpPost("{user2Id}")]
        public async Task<ActionResult> Post(string user2Id)
        {
            var user = await userService.GetUser();

            var userURL = await userManager.FindByIdAsync(user2Id);

            if (user is null)
            {
                return NotFound();
            }

            if (userURL is null)
            {
                return NotFound();
            }

            var chatDB = await context.Chats
                .FirstOrDefaultAsync(u => (u.UserId == user.Id && u.User2Id == userURL.Id) ||
                (u.UserId == userURL.Id && u.User2Id == user.Id));
                

            if (chatDB is not null)
            {
                return Ok(mapper.Map<ChatDTO>(chatDB));
            }

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
