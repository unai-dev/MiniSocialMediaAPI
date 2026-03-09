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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> Get()
        {
            var posts = await context.Posts
                .Include(l => l.Likes)
                .Include(u => u.UserId)
                .Include(c => c.Coments)
                .ToListAsync();
            return Ok(mapper.Map<IEnumerable<PostDTO>>(posts));
        }

        [HttpGet("{id}", Name ="GetPost")]
        public async Task<ActionResult<PostDTO>> Get(Guid id)
        {
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

        [HttpPost]
        public async Task<ActionResult> Post(AddPostDTO addPostDTO)
        {
            var user = await userService.GetUser();
            if (user is null)
            {
                return BadRequest();
            }

            var post = mapper.Map<Post>(addPostDTO);
            post.UserId = user.Id;

            context.Add(post);
            await context.SaveChangesAsync();

            var postDTO = mapper.Map<PostDTO>(post);

            return CreatedAtRoute("GetPost", new { id = post.Id }, postDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var user = await userService.GetUser();

            if (user is null)
            {
                return BadRequest();
            }

            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (post!.UserId != user.Id)
            {
                return Forbid();
            }

            context.Remove(post);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
