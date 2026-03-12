using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSocialMediaAPI.Data;
using MiniSocialMediaAPI.DTOs.Group;
using MiniSocialMediaAPI.Entities;
using MiniSocialMediaAPI.Services;
using System.Text.RegularExpressions;

namespace MiniSocialMediaAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de grupos en la aplicación.
    /// Permite crear, consultar, unirse y salir de grupos.
    /// Los administradores pueden eliminar grupos y remover usuarios de ellos.
    /// Todos los endpoints requieren autenticación.
    /// </summary>
    [ApiController]
    [Route("v1/api/groups")]
    [Authorize]
    public class GroupController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public GroupController(ApplicationDbContext context, IMapper mapper,
            IUserService userService, UserManager<User> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userService = userService;
            this.userManager = userManager;
        }

        /// <summary>
        /// Obtiene la lista de todos los grupos disponibles en la aplicación.
        /// Incluye información de todos los usuarios miembros de cada grupo.
        /// </summary>
        /// <returns>Lista de todos los grupos con sus miembros</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> Get()
        {
            var groups = await context.Groups.Include(u => u.Users).ToListAsync();
            return Ok(mapper.Map<IEnumerable<GroupDTO>>(groups));
        }

        /// <summary>
        /// Obtiene los detalles de un grupo específico por su ID.
        /// Incluye la lista de todos los miembros del grupo.
        /// </summary>
        /// <param name="id">ID único del grupo a obtener</param>
        /// <returns>Datos del grupo con sus miembros o error 404 si no existe</returns>
        [HttpGet("{id:int}", Name ="GetGroup")]
        public async Task<ActionResult<GroupDTO>> Get(int id)
        {
            var group = await context.Groups.Include(u => u.Users).FirstOrDefaultAsync(x => x.Id == id);
            if (group is null)
            {
                return NotFoundMessage();
            }

            return Ok(mapper.Map<GroupDTO>(group));
        }

        /// <summary>
        /// Crea un nuevo grupo en la aplicación.
        /// El grupo se crea con la información proporcionada en el DTO.
        /// </summary>
        /// <param name="addGroupDTO">Información del nuevo grupo (nombre, descripción, etc.)</param>
        /// <returns>Datos del grupo creado con su ID único</returns>
        [HttpPost]
        public async Task<ActionResult> Post(AddGroupDTO addGroupDTO)
        {
            // Mapea el DTO a la entidad Group
            // Nota: Se utiliza Entities.Group para evitar confusión con la clase System.Group
            var group = mapper.Map<Entities.Group>(addGroupDTO);
            
            context.Add(group);
            await context.SaveChangesAsync();

            var groupDTO = mapper.Map<GroupDTO>(group);

            return CreatedAtRoute("GetGroup", new { id = group.Id }, groupDTO);
        }

        /// <summary>
        /// El usuario autenticado se une a un grupo existente.
        /// Después de ejecutar este endpoint, el usuario será miembro del grupo.
        /// </summary>
        /// <param name="groupId">ID del grupo al que se desea unirse</param>
        /// <returns>Mensaje de confirmación si se une exitosamente</returns>
        [HttpPost("{groupId}/join")]
        public async Task<ActionResult> JoinGroup(int groupId)
        {
            var user = await userService.GetUser();

            if (user is null)
            {
                return Unauthorized();
            }

            var group = await context.Groups.FirstOrDefaultAsync(x => x.Id == groupId);

            if (group is null)
            {
                return NotFoundMessage();
            }

            // Agrega el usuario a la lista de miembros del grupo
            group.Users.Add(user);
            await context.SaveChangesAsync();

            return Ok("Te has unido al grupo");
        }

        /// <summary>
        /// El usuario autenticado abandona un grupo del que es miembro.
        /// Después de ejecutar este endpoint, el usuario deja de ser miembro del grupo.
        /// </summary>
        /// <param name="groupId">ID del grupo que se desea abandonar</param>
        /// <returns>Mensaje de confirmación si abandona el grupo exitosamente</returns>
        [HttpPost("{groupId}/leave")]
        public async Task<ActionResult> LeaveGroup(int groupId)
        {
            var user = await userService.GetUser();

            if (user is null)
            {
                return Unauthorized();
            }

            var group = await context.Groups.FirstOrDefaultAsync(x => x.Id == groupId);

            if (group is null)
            {
                return NotFoundMessage();
            }

            // Remueve el usuario de la lista de miembros del grupo
            group.Users.Remove(user);
            await context.SaveChangesAsync();

            return Ok($"Has salido del grupo {group.Name} con éxito");
        }

        /// <summary>
        /// Remueve un usuario específico de un grupo.
        /// Esta operación generalmente la realiza un administrador o moderador del grupo.
        /// </summary>
        /// <param name="groupId">ID del grupo del que se desea remover al usuario</param>
        /// <param name="userId">ID del usuario a remover del grupo</param>
        /// <returns>Mensaje de confirmación si el usuario se remueve exitosamente</returns>
        [HttpPost("{groupId}/remove/{userId}")]
        public async Task<ActionResult> RemoveUserGroup(int groupId, string userId)
        {
            // Busca el usuario por su ID
            var user = await userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return BadRequest();
            }

            var group = await context.Groups.FirstOrDefaultAsync(x => x.Id == groupId);

            if (group is null)
            {
                return NotFoundMessage();
            }

            // Remueve el usuario del grupo
            group.Users.Remove(user);
            await context.SaveChangesAsync();

            return Ok($"{user.UserName} eliminado con éxito");
        }

        /// <summary>
        /// Elimina un grupo completamente de la aplicación.
        /// Todos los mensajes y relaciones del grupo se eliminarán también.
        /// Esta operación generalmente requiere permisos de administrador.
        /// </summary>
        /// <param name="id">ID del grupo a eliminar</param>
        /// <returns>204 No Content si se elimina exitosamente</returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var groupDeleted = await context.Groups.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (groupDeleted == 0)
            {
                return NotFoundMessage();
            }

            return NoContent();
        }

        /// <summary>
        /// Método interno para retornar un error consistente cuando no se encuentra un grupo.
        /// Retorna un problema de validación con un mensaje descriptivo.
        /// </summary>
        /// <returns>Respuesta de error 400 con mensaje de grupo no encontrado</returns>
        private ActionResult NotFoundMessage()
        {
            ModelState.AddModelError(string.Empty, "Grupo no encontrado");
            return ValidationProblem();
        }
    }
}
