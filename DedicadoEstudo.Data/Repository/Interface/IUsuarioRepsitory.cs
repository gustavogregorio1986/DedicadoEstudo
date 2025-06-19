using DedicadoEstudo.Dominio.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DedicadoEstudo.Data.Repository.Interface
{
    public interface IUsuarioRepsitory
    {
        Task<Usuario> AdicionarUsuario(Usuario usuario);
    }
}
