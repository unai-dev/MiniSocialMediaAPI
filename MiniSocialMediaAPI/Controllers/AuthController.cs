using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MiniSocialMediaAPI.DTOs.Auth;
using MiniSocialMediaAPI.DTOs.User;
using MiniSocialMediaAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniSocialMediaAPI.Controllers
{
    /// <summary>
    /// Controlador de autenticación y autorización.
    /// Gestiona el registro de usuarios, login y asignación de permisos de administrador.
    /// Todos los endpoints retornan tokens JWT para autenticarse en la API.
    /// </summary>
    [ApiController]
    [Route("v1/api/auth")]
    public class AuthController: ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        /// <summary>
        /// Registra un nuevo usuario en la aplicación.
        /// Crea el usuario con nombre de usuario, email y contraseña.
        /// Retorna un token JWT válido por 1 hora si el registro es exitoso.
        /// </summary>
        /// <param name="userCredentials">Objeto con userName, email y password del nuevo usuario</param>
        /// <returns>Token JWT y fecha de expiración del token</returns>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(UserCredentialsDTO userCredentials)
        {
            var user = new User
            {
                UserName = userCredentials.UserName,
                Email = userCredentials.Email,
            };

            // Intenta crear el usuario con la contraseña proporcionada
            var result = await userManager.CreateAsync(user, userCredentials.Password!);

            if (result.Succeeded)
            {
                // Si se crea exitosamente, genera y retorna un token JWT
                var responseDTO = await BuildToken(userCredentials);
                return responseDTO;
            }
            else
            {
                // Si hay errores, los agrega al estado del modelo
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }

                return ValidationProblem();
            }
        }

        /// <summary>
        /// Autentica un usuario existente con sus credenciales.
        /// Verifica que el email y contraseña sean correctos.
        /// Retorna un token JWT si la autenticación es exitosa.
        /// </summary>
        /// <param name="userCredentials">Email y contraseña del usuario</param>
        /// <returns>Token JWT válido por 1 hora</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(UserCredentialsDTO userCredentials)
        {
            // Busca el usuario por email
            var user = await userManager.FindByEmailAsync(userCredentials.Email);

            if (user is null)
            {
                return BadRequest();
            }

            // Verifica que la contraseña sea correcta
            var result = await signInManager.CheckPasswordSignInAsync(user, userCredentials.Password!, lockoutOnFailure: false);

            if (result.Succeeded) 
                return await BuildToken(userCredentials);
            else 
                return BadRequest();
        }

        /// <summary>
        /// Asigna permisos de administrador a un usuario existente.
        /// Solo debe ser usado por administradores del sistema.
        /// Agrega el claim "admin" al usuario especificado.
        /// </summary>
        /// <param name="claimDTO">Email del usuario al que se le asignarán permisos de admin</param>
        /// <returns>204 No Content si la operación es exitosa</returns>
        [HttpPost("admin")]
        public async Task<ActionResult> AddAmdmin(EditClaimDTO claimDTO)
        {
            // Busca el usuario por email
            var user = await userManager.FindByEmailAsync(claimDTO.Email);
            if (user is null)
            {
                return BadRequest();
            }

            // Agrega el claim de administrador al usuario
            await userManager.AddClaimAsync(user, new Claim("admin", "true"));
            return NoContent();
        }

        /// <summary>
        /// Método interno que genera un token JWT para el usuario.
        /// Incluye el email y todos los claims asociados al usuario.
        /// El token es válido por 1 hora desde su creación.
        /// </summary>
        /// <param name="userCredentials">Credenciales del usuario autenticado</param>
        /// <returns>Token JWT codificado y fecha de expiración</returns>
        private async Task<AuthResponseDTO> BuildToken(UserCredentialsDTO userCredentials)
        {
            // Crea los claims iniciales con el email del usuario
            var claims = new List<Claim>
            {
                new Claim("email", userCredentials.Email)
            };

            // Obtiene el usuario y todos sus claims de la base de datos
            var user = await userManager.FindByEmailAsync(userCredentials.Email);
            var claimsDB = await userManager.GetClaimsAsync(user!);

            // Agrega los claims adicionales (como "admin" si es administrador)
            claims.AddRange(claimsDB);

            // Configura la clave de firma y las credenciales del token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(1);

            // Crea el token JWT con los claims y credenciales
            var securityToken = new JwtSecurityToken(
                issuer: null, audience: null, claims: claims, signingCredentials: credentials, expires: expires
            );

            // Convierte el token a string codificado
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            // Retorna el token y su fecha de expiración
            return new AuthResponseDTO
            {
                Token = token,
                Expires = expires
            };
        }
    }
}
