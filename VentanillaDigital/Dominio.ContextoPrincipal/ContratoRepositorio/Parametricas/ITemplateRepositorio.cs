using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.ContextoPrincipal.ContratoRepositorio
{
    public interface ITemplateRepositorio : IRepositorioBase<Parametrica>, IDisposable
    {
        InformacionUsuarioPlantilla ObtenerInformacionUsuario(string Email);
        string ObtenerTemplate(string Codigo);
    }
}
