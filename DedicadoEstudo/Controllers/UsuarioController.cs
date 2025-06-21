using AutoMapper;
using DedicadoEstudo.Data.DTO;
using DedicadoEstudo.Dominio.Dominio;
using DedicadoEstudo.Service.CRiptografia;
using DedicadoEstudo.Service.JWT;
using DedicadoEstudo.Service.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DedicadoEstudo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper, IConfiguration config)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
            _config = config;
        }


        [HttpPost]
        [Route("AdicionarUsuario")]
        public async Task<JsonResult> AdicionarUsuario([FromBody] UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO == null)
            {
                return new JsonResult("Usuário inválido") { StatusCode = StatusCodes.Status400BadRequest };
            }

            // Validações básicas - não deixar campos em branco
            if (string.IsNullOrWhiteSpace(usuarioDTO.Senha))
            {
                return new JsonResult("Senha é obrigatória") { StatusCode = StatusCodes.Status400BadRequest };
            }

            if (string.IsNullOrWhiteSpace(usuarioDTO.Email))
            {
                return new JsonResult("Email é obrigatório") { StatusCode = StatusCodes.Status400BadRequest };
            }

            // Outras validações que quiser, ex: perfil, nome...

            // Criptografar a senha
            var senhaHash = PasswordHasher.HashPassword(usuarioDTO.Senha);

            // Mapear DTO para entidade
            var usuario = _mapper.Map<Usuario>(usuarioDTO);
            usuario.SenhaHash = senhaHash; // Substitui a senha original pela criptografada

            var usuarioAdicionado = await _usuarioService.AdicionarUsuario(usuario);

            if (usuarioAdicionado == null)
            {
                return new JsonResult("Erro ao adicionar usuário") { StatusCode = StatusCodes.Status500InternalServerError };
            }

            return new JsonResult(usuarioAdicionado) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioDTO dto)
        {
            var usuario = await _usuarioService.ObterPorEmail(dto.Email);

            if (usuario == null)
                return Unauthorized(new { mensagem = "Email ou senha inválidos" });

            bool senhaValida;
            try
            {
                senhaValida = BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash);
            }
            catch
            {
                return Unauthorized(new { mensagem = "Erro ao validar senha" });
            }

            if (!senhaValida)
                return Unauthorized(new { mensagem = "Email ou senha inválidos" });

            var token = JwtHelper.GerarToken(usuario, _config["Jwt:Key"]);

            return Ok(new { token });
        }
    }
}
