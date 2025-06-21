using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DedicadoEstudo.Controllers
{
    [Authorize(Roles = "Usu")]
    [ApiController]
    [Route("Usu")]
    public class UsuController : ControllerBase
    {
        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            return Ok("Bem-vindo, usuário com role 'Usu'");
        }
    }
}
