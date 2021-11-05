using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;
using System;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato
{
    public interface INotariasUsuarioServicio : IDisposable
    {
        Task<int> CrearNotariaUsuario(NotariaUsuarioCreateDTO notaria);
        Task<ContactoFuncionarioReturnDTO> ObtenerDatosDeContacto(string usuarioId);
    }
}
