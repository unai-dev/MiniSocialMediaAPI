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
    /// <summary>
    /// Controlador para la gestión de usuarios.
    /// Permite obtener información de usuarios, actualizar perfil y otros datos personales.
    /// Todos los endpoints requieren autenticación con un token JWT válido.
    /// </summary>
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

        /// <summary>
        /// Obtiene la lista de todos los usuarios registrados en la aplicación.
        /// Solo los administradores pueden acceder a este endpoint.
        /// Retorna información completa de cada usuario incluyendo grupos y posts.
        /// </summary>
        /// <returns>Lista de todos los usuarios del sistema</returns>
        [HttpGet]
        [Authorize(Policy ="admin")]
        public async Task<IEnumerable<UserDTO>> Get()
        {
            // Obtiene todos los usuarios con sus grupos y posts relacionados
            var users = await userManager.Users
                .Include(g => g.Groups)
                .Include(p => p.Posts)
                .ToListAsync();

            // Mapea las entidades a DTOs para la respuesta
            var usersDTO = (mapper.Map<IEnumerable<UserDTO>>(users));
            return usersDTO;
        }

        /// <summary>
        /// Obtiene la información del usuario autenticado actualmente.
        /// Utiliza el email del token JWT para identificar al usuario.
        /// </summary>
        /// <returns>Datos del usuario actual autenticado</returns>
        [HttpGet("me")]
        public async Task<ActionResult<UserDTO>> GetMe()
        {
            // Obtiene el usuario desde el servicio (busca por email del token)
            var user = await userService.GetUser();

            if (user is null) 
                return Unauthorized();

            // Mapea la entidad a DTO
            var userDTO = mapper.Map<UserDTO>(user);

            return Ok(userDTO);
        }

        /// <summary>
        /// Actualiza los datos del perfil del usuario autenticado.
        /// Permite cambiar el nombre de usuario y la fecha de nacimiento.
        /// Solo el usuario autenticado puede actualizar su propia información.
        /// </summary>
        /// <param name="updateUserDTO">Objeto con los datos a actualizar (nombre de usuario y/o fecha de nacimiento)</param>
        /// <returns>204 No Content si la actualización es exitosa</returns>
        [HttpPut("update")]
        public async Task<ActionResult> Update(UpdateUserDTO updateUserDTO)
        {
            // Obtiene el usuario actual del token JWT
            var user = await userService.GetUser();

            if (user is null)
            {
                return BadRequest();
            }

            // Actualiza el nombre de usuario si se proporciona
            if (updateUserDTO.UserName != null)
            {
                user.UserName = updateUserDTO.UserName;
            }

            // Actualiza la fecha de nacimiento si se proporciona
            if (updateUserDTO.Birthdate.HasValue)
            {
                user.Birthdate = updateUserDTO.Birthdate.Value;
            }

            // Guarda los cambios en la base de datos
            await userManager.UpdateAsync(user);

            return NoContent();
        }
    }
}
