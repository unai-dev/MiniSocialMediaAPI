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
    [ApiController]
    [Route("v1/api/posts/{postId}/coments")]
    [Authorize]
    public class ComentController: ControllerBase
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComentDTO>>> Get(Guid postId)
        {
            var post = await context.Posts.AnyAsync(x => x.Id == postId);

            if (!post)
            {
                return NotFound();
            }

            var coments = await context.Coments.Where(x => x.PostId == postId).ToListAsync();

            if (coments is null)
            {
                return Ok();
            }

            return Ok(mapper.Map<IEnumerable<ComentDTO>>(coments));
        }

        [HttpPost]
        public async Task<ActionResult> Post(Guid postId, AddComentDTO addComentDTO)
        {
            var post = await context.Posts.AnyAsync(x => x.Id == postId);

            if (!post)
            {
                return NotFound();
            }

            var user = await userService.GetUser();

            if (user is null)
            {
                return BadRequest();
            }

            var coment = mapper.Map<Coments>(addComentDTO);
            coment.PostId = postId;
            coment.UserId = user.Id;

            context.Add(coment);
            await context.SaveChangesAsync();

            return Ok(mapper.Map<ComentDTO>(coment));
        }

    }
}
