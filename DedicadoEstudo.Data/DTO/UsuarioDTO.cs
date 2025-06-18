using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DedicadoEstudo.Data.DTO
{
    public class UsuarioDTO
    {
        public Guid Id { get; set; }

        public string? Email { get; set; }

        public string? Senha { get; set; }

        public string? Perfil { get; set; }
    }
}
