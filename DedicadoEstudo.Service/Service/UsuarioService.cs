using BCrypt.Net;
using DedicadoEstudo.Data.Repository.Interface;
using DedicadoEstudo.Dominio.Dominio;
using DedicadoEstudo.Service.CRiptografia;
using DedicadoEstudo.Service.JWT;
using DedicadoEstudo.Service.Service.Interface;
using Microsoft.Extensions.Configuration;
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
        private readonly PasswordHasher _passwordHasher;
        private readonly IConfiguration _config;


        public UsuarioService(IUsuarioRepsitory usuarioRepository, PasswordHasher passwordHasher, IConfiguration config)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _config = config;
        }

        public async Task<Usuario> AdicionarUsuario(Usuario usuario)
        {
            return await _usuarioRepository.AdicionarUsuario(usuario);
        }

        public async Task<string> Login(string email, string senha)
        {
            var usuario = await _usuarioRepository.ObterPorEmail(email);
            if (usuario == null)
                return null;

            try
            {
                bool senhaValida = BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash);
                if (!senhaValida)
                    return null;

                return JwtHelper.GerarToken(usuario, _config["Jwt:Key"]);
            }
            catch (BCrypt.Net.SaltParseException ex)
            {
                // Logue o erro para investigar
                return null;
            }
        }

        public async Task<Usuario> ObterPorEmail(string email)
        {
            return await _usuarioRepository.ObterPorEmail(email);
        }
    }
}
