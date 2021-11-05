using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Vista;
using Dominio.Nucleo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dominio.ContextoPrincipal.ContratoRepositorio
{
    public interface INotariasUsuarioRepositorio:IRepositorioBase<NotariaUsuarios>, IDisposable
    {
        public Task<PersonaDatos[]> ObtenerDatosContacto(string usuarioId, string include ="");

        public Task<PersonaDatos[]> ObtenerDatosContacto(string usuarioId, 
            Expression<Func<PersonaDatos,bool>> selector, string include = "");

        public Task<(DatosNotaria, string, DatosNotario, string)> ObtenerNotariaUsuarioActa(string userEmail);
    }
}
