using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Infraestructura.Transversal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato
{
    public interface IMaquinaServicio : IDisposable
    {
        Task<MaquinaDTO> CrearMaquina(MaquinaCreateDTO Maquina);
        Task<PaginableResponse<MaquinaConfiguracionReturnDTO>> ObtenerConfiguracionesUsuario(ConfiguracionesNotariaRequestDTO configuracionesNotariaRequestDTO);
        Task<MaquinaConfiguracionReturnDTO> ConsultarMaquina(string mac);
    }
}
