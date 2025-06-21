using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DedicadoEstudo.Data.DTO
{
    public class UsuarioDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório")]
        public string? SenhaHash { get; set; }

        [Required(ErrorMessage = "Role é obrigatório")]
        public string? Role { get; set; }
    }
}
