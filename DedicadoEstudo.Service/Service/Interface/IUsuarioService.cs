using DedicadoEstudo.Dominio.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DedicadoEstudo.Service.Service.Interface
{
    public interface IUsuarioService
    {
        Task<Usuario> AdicionarUsuario(Usuario usuario);
    }
}
