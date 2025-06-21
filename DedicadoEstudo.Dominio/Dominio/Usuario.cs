using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DedicadoEstudo.Dominio.Dominio
{
    public class Usuario
    {
        public Guid Id { get; set; }

        public string? Email { get; set; }

        public string? SenhaHash { get; set; }

        public string? Role { get; set; }
    }
}
