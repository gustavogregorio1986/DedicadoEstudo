using DedicadoEstudo.Data.Repository.Interface;
using DedicadoEstudo.Dominio.Dominio;
using DedicadoEstudo.Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DedicadoEstudo.Service.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepsitory _usuarioRepository;

        public UsuarioService(IUsuarioRepsitory usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario> AdicionarUsuario(Usuario usuario)
        {
            return await _usuarioRepository.AdicionarUsuario(usuario);
        }

        public async Task<Usuario> ObterPorEmail(string email)
        {
            return await _usuarioRepository.ObterPorEmail(email);
        }
    }
}
