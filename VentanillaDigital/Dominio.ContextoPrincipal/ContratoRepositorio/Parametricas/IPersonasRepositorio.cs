using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;
using Dominio.Nucleo.Entidad;

namespace Dominio.ContextoPrincipal.ContratoRepositorio
{
    public interface IPersonasRepositorio : IRepositorioBase<Persona>, IDisposable
    {
        Task<Persona> Personas_Obtener(string numeroDocumento);
        Task<Persona> ObtenerPorCorreo(string correo);
    }
}
