using Aplicacion.ContextoPrincipal.Modelo;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato
{
    public interface IPersonasServicio : IDisposable
    {
        Task<int> CrearActualizarPersona(PersonaCreateOrUpdateDTO persona);
        Task<int> EliminarUsuarios(PersonaDeleteRequestDTO persona);
        Task<Persona> ConsultarPersonaPorCorreo(string correo);
    }
}
