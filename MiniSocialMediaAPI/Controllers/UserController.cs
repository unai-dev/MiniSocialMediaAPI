using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSocialMediaAPI.DTOs.User;
using MiniSocialMediaAPI.Entities;
using MiniSocialMediaAPI.Services;

namespace MiniSocialMediaAPI.Controllers
{
    [ApiController]
    [Route("v1/api/users")]
    [Authorize]
    public class UserController: ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public UserController(IUserService userService, IMapper mapper, UserManager<User> userManager)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        /**
         * El unico que puede consultar los usuarios, es el administrador
         * Esto puede no tener sentido, dependiendo la aplicacion
         * Por ahora mantenemos seguridad
         * 
         */
        [HttpGet]
        [Authorize(Policy ="admin")]
        public async Task<IEnumerable<UserDTO>> Get()
        {
            var users = await userManager.Users.Include(g => g.Groups).ToListAsync();
            var usersDTO = (mapper.Map<IEnumerable<UserDTO>>(users));
            return usersDTO;
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserDTO>> GetMe()
        {
            var user = await userService.GetUser();

            if (user is null) return Unauthorized();

            var userDTO = mapper.Map<UserDTO>(user);

            return Ok(userDTO);
        }
    }
}
