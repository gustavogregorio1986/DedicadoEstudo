using DedicadoEstudo.Dominio.Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DedicadoEstudo.Data.Infraestrutura
{
    public class DedicadoEstudoContext : DbContext
    {
        public DedicadoEstudoContext(DbContextOptions<DedicadoEstudoContext> options)
            : base(options)
        {
        }
        // DbSet properties for your entities go here, e.g.:
         public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DedicadoEstudoContext).Assembly);
            base.OnModelCreating(modelBuilder);
            // Additional model configuration can go here
        }
    }
}
