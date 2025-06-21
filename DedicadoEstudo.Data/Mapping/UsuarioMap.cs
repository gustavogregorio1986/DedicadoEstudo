using DedicadoEstudo.Dominio.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DedicadoEstudo.Data.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("tb_Usuario");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(u => u.SenhaHash)
                .HasColumnName("Senha")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Role)
               .HasColumnName("Role")
               .IsRequired()
               .HasMaxLength(40);
        }
    }
}
