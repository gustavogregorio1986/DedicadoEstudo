using AutoMapper;
using DedicadoEstudo.Data.DTO;
using DedicadoEstudo.Dominio.Dominio;
using DedicadoEstudo.Service.CRiptografia;
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

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }


        [HttpPost]
        [Route("AdicionarUsuario")]
        public async Task<JsonResult> AdicionarUsuario([FromBody] UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO == null)
            {
                return new JsonResult("Usuário inválido") { StatusCode = StatusCodes.Status400BadRequest };
            }

            // Criptografar a senha
            var senhaHash = PasswordHasher.HashPassword(usuarioDTO.Senha);

            // Mapear DTO para entidade
            var usuario = _mapper.Map<Usuario>(usuarioDTO);
            usuario.Senha = senhaHash; // Substitui a senha original pela criptografada

            var usuarioAdicionado = await _usuarioService.AdicionarUsuario(usuario);

            if (usuarioAdicionado == null)
            {
                return new JsonResult("Erro ao adicionar usuário") { StatusCode = StatusCodes.Status500InternalServerError };
            }

            return new JsonResult(usuarioAdicionado) { StatusCode = StatusCodes.Status201Created };
        }


    }
}
