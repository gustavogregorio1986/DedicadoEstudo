using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DedicadoEstudo.Controllers
{
    [Authorize(Roles = "Adm")]
    [ApiController]
    [Route("Adm")]
    public class AdmController : ControllerBase
    {
        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            return Ok("Bem-vindo, usuário com role 'Adm'");
        }

    }
}
