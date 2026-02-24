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
    [ApiController]
    [Route("v1/api/groups")]
    [Authorize]
    public class GroupController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public GroupController(ApplicationDbContext context, IMapper mapper, IUserService userService )
        {
            this.context = context;
            this.mapper = mapper;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> Get()
        {
            var groups = await context.Groups.ToListAsync();
            return Ok(mapper.Map<IEnumerable<GroupDTO>>(groups));
        }

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

        [HttpPost]
        public async Task<ActionResult> Post(AddGroupDTO addGroupDTO)
        {
            /**
             *  ENTITIES.GROUP --> uso entities.group(referencia a la entidad grupo) 
             *  por el hecho que en ASP.NET CORE
             *  ya existe una clase llamada "Group", en este caso evitamos la confusion
             *  entre ambas
             */
            var group = mapper.Map<Entities.Group>(addGroupDTO);
            context.Add(group);
            await context.SaveChangesAsync();

            var groupDTO = mapper.Map<GroupDTO>(group);

            return CreatedAtRoute("GetGroup", new { id = group.Id }, groupDTO);

        }

        /**
         * POST -> Join Group
         * 
         */
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

            group.Users.Add(user);
            await context.SaveChangesAsync();

            return Ok("Te has unido al grupo");
        }

        /**
         * TODO -> agregar metodo PUT o PATCH
         * PUT -> obligamos a indicar cada campo
         * PATCH -> hacemos opcionales y solamente mandamos el campo a actualizar
         * CONCLUSION -> patch -> mas efectivo; put -> mas restrictivo
         */

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

        /**
         * Funcion para lanzar un error cuando no encontramos un grupo
         * return ValidationProblem()
         */
        private ActionResult NotFoundMessage()
        {
            ModelState.AddModelError(string.Empty, "Group not found");
            return ValidationProblem();
        }
    }
}
