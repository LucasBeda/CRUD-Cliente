using CRUD_Cliente.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CRUD_Cliente.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private IConfiguration _config;
        private readonly ILogger<TokenController> _log;
        public TokenController(ILogger<TokenController> logger, IConfiguration config)
        {
            _log = logger;
            _config = config;
        }

        [HttpPost]
        public IActionResult Login([FromBody] Usuario login)
        {
            try
            {
                bool usuarioValidado = new Usuario().ValidarUsuario(login);

                if (!usuarioValidado)
                    return Unauthorized();

                return Ok(GerarTokenJWT());
            }
            catch (Exception ex)
            {
                _log.LogError($"Login - Erro: {ex.Message}");
                throw;
            }
        }

        private string GerarTokenJWT()
        {
            string issuer = _config.GetValue<string>("Jwt:Issuer");
            DateTime timeout = DateTime.Now.AddMinutes(_config.GetValue<double>("Jwt:TimeOut"));
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:Key")));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken securityToken = new JwtSecurityToken(issuer: issuer, expires: timeout, signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
