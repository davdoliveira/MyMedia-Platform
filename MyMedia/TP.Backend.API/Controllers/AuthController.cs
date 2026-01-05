using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TP.Backend.API.DTOs;
using TP.Backend.Data.Entidades;

namespace TP.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                NomeCompleto = model.NomeCompleto,
                Tipo = (UserType)model.Tipo, // Assumindo que model.Tipo é int, fazemos cast para o Enum
                Estado = UserStatus.Pendente // Regra de negócio: Começa Pendente
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Opcional: Adicionar ao Role do Identity também, para facilitar filtros futuros
                // await _userManager.AddToRoleAsync(user, user.Tipo.ToString());
                return Ok(new { Message = "Registado com sucesso! A aguardar aprovação." });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            // 1. Verificar se o user existe
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return Unauthorized("Credenciais inválidas.");

            // 2. Verificar Password
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized("Credenciais inválidas.");

            // 3. --- NOVA VERIFICAÇÃO DE ESTADO ---
            // Se não estiver Ativo (ou seja, se for Pendente ou Suspenso), bloqueia.
            if (user.Estado != UserStatus.Ativo)
            {
                return Unauthorized("A sua conta está pendente de aprovação ou suspensa. Contacte o suporte.");
            }

            // 4. Se passou tudo, gera o Token
            var tokenString = GenerateJwtToken(user);
            return Ok(new { token = tokenString, nome = user.NomeCompleto });
        }

        // --- MÉTODO EM FALTA QUE GERA O TOKEN ---
        private string GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Vai buscar a chave ao appsettings.json
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            // Define o que vai dentro do Token (Claims)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("NomeCompleto", user.NomeCompleto),
                new Claim(ClaimTypes.Role, user.Tipo.ToString()) // Guardamos se é Cliente/Fornecedor
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Token válido por 7 dias
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}