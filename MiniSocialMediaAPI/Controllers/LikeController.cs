using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSocialMediaAPI.Data;
using MiniSocialMediaAPI.Entities;
using MiniSocialMediaAPI.Services;

namespace MiniSocialMediaAPI.Controllers
{
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

        [HttpPost]
        public async Task<ActionResult> Post(Guid postId)
        {
            var user = await userService.GetUser();

            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == postId);

            if (post is null)
            {
                return NotFound();
            }

            var likeExists = await context.Likes.AnyAsync(u => u.UserId == user.Id && u.PostId == postId);

            if (likeExists)
            {
                return BadRequest("El like ya existe");
            }


            var like = new Like
            {
                PostId = post.Id,
                UserId = user.Id

            };

            context.Likes.Add(like);
            await context.SaveChangesAsync();

            return Ok();

        }
    }
}
