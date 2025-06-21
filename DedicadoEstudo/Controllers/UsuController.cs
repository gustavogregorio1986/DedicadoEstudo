using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DedicadoEstudo.Controllers
{
    [Authorize(Roles = "usu")]
    [ApiController]
    [Route("usu")]
    public class UsuController : ControllerBase
    {
        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            return Ok("Bem-vindo, usuário com role 'Usu'");
        }
    }
}
