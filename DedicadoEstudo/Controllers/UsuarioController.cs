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
                return new JsonResult("Usuário inválido") { StatusCode = StatusCodes.Status400BadRequest };

            if (string.IsNullOrWhiteSpace(usuarioDTO.Senha))
                return new JsonResult("Senha é obrigatória") { StatusCode = StatusCodes.Status400BadRequest };

            if (string.IsNullOrWhiteSpace(usuarioDTO.Email))
                return new JsonResult("Email é obrigatório") { StatusCode = StatusCodes.Status400BadRequest };

            // Verifica se email já existe (melhora a UX)
            var usuarioExistente = await _usuarioService.ObterPorEmail(usuarioDTO.Email);
            if (usuarioExistente != null)
                return new JsonResult("Email já cadastrado") { StatusCode = StatusCodes.Status400BadRequest };

            // Criptografa a senha
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(usuarioDTO.Senha);

            // Mapear DTO para entidade (assumindo AutoMapper)
            var usuario = _mapper.Map<Usuario>(usuarioDTO);
            usuario.SenhaHash = senhaHash; // Substitui a senha original pela hash

            // Salvar no banco
            var usuarioAdicionado = await _usuarioService.AdicionarUsuario(usuario);

            if (usuarioAdicionado == null)
                return new JsonResult("Erro ao adicionar usuário") { StatusCode = StatusCodes.Status500InternalServerError };

            // Retorna o usuário criado (pode ocultar senha e outros dados sensíveis)
            return new JsonResult(new
            {
                usuarioAdicionado.Id,
                usuarioAdicionado.Email,
                usuarioAdicionado.Role // se tiver
            })
            { StatusCode = StatusCodes.Status201Created };
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioDTO dto)
        {
            var usuario = await _usuarioService.ObterPorEmail(dto.Email);

            if (usuario == null)
                return Unauthorized(new { mensagem = "Email ou senha inválidos." });

            if (string.IsNullOrWhiteSpace(usuario.SenhaHash) || !usuario.SenhaHash.StartsWith("$2"))
                return Unauthorized(new { mensagem = "Senha inválida no banco." });

            bool senhaValida = false;
            try
            {
                senhaValida = BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao validar senha: " + ex.Message);
                return Unauthorized(new { mensagem = "Erro ao validar senha." });
            }

            if (!senhaValida)
                return Unauthorized(new { mensagem = "Email ou senha inválidos." });

            var token = JwtHelper.GerarToken(usuario, _config["Jwt:Key"]);
            return Ok(new { token });
        }


    }
}
