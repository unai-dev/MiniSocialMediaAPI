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

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(UserCredentialsDTO userCredentials)
        {
            var user = new User
            {
                UserName = userCredentials.UserName,
                Email = userCredentials.Email,
            };

            var result = await userManager.CreateAsync(user, userCredentials.Password!);

            if (result.Succeeded)
            {
                var responseDTO = await BuildToken(userCredentials);
                return responseDTO;
            }
            else
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }

                return ValidationProblem();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(UserCredentialsDTO userCredentials)
        {
            var user = await userManager.FindByEmailAsync(userCredentials.Email);

            if (user is null)
            {
                return BadRequest();
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, userCredentials.Password!, lockoutOnFailure: false);

            if (result.Succeeded) return await BuildToken(userCredentials);
            else return BadRequest();
        }

        [HttpPost("admin")]
        public async Task<ActionResult> AddAmdmin(EditClaimDTO claimDTO)
        {
            var user = await userManager.FindByEmailAsync(claimDTO.Email);
            if (user is null)
            {
                return BadRequest();
            }

            await userManager.AddClaimAsync(user, new Claim("admin", "true"));
            return NoContent();
        }

        private async Task<AuthResponseDTO> BuildToken(UserCredentialsDTO userCredentials)
        {
            var claims = new List<Claim>
            {
                new Claim("email", userCredentials.Email)
            };

            var user = await userManager.FindByEmailAsync(userCredentials.Email);
            var claimsDB = await userManager.GetClaimsAsync(user!);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(1);

            var securityToken = new JwtSecurityToken(
                issuer: null, audience: null, claims: claims, signingCredentials: credentials, expires: expires
            );

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new AuthResponseDTO
            {
                Token = token,
                Expires = expires
            };
        }
    }
}
