using DedicadoEstudo.Data.Infraestrutura;
using DedicadoEstudo.Data.Repository.Interface;
using DedicadoEstudo.Dominio.Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DedicadoEstudo.Data.Repository
{
    public class UsuarioRepository : IUsuarioRepsitory
    {
        private readonly DedicadoEstudoContext _db;

        public UsuarioRepository(DedicadoEstudoContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Usuario> AdicionarUsuario(Usuario usuario)
        {
            try
            {
                await _db.Usuarios.AddAsync(usuario);
                await _db.SaveChangesAsync();
                return usuario;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while adding the user.", ex);
            }
        }

        public async Task<Usuario> ObterPorEmail(string email)
        {
            return await _db.Usuarios
        .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
