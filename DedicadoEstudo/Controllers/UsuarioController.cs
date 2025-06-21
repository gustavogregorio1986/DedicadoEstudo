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
        private readonly PasswordHasher _criptografia;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper, PasswordHasher criptografia)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
            _criptografia = criptografia;
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
        public async Task<IActionResult> Login([FromBody] UsuarioDTO usuarioDTO)
        {
            var usuario = await _usuarioService.ObterPorEmail(usuarioDTO.Email);

            if (usuario == null)
                return Unauthorized("Usuário não encontrado.");

            if (string.IsNullOrEmpty(usuario.SenhaHash))
                return StatusCode(500, "Senha não cadastrada para o usuário.");

            bool senhaValida = BCrypt.Net.BCrypt.Verify(usuarioDTO.Senha, usuario.SenhaHash);

            if (!senhaValida)
                return Unauthorized("Senha inválida.");

            // Login válido - prossiga com geração de token ou retorno
            return Ok("Login efetuado com sucesso.");
        }


    }
}
