using AutoMapper;
using DedicadoEstudo.Data.DTO;
using DedicadoEstudo.Dominio.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DedicadoEstudo.Data.AutoMapper
{
    public class UsuarioProfile : Profile 
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDTO>();
        }
    }
}
