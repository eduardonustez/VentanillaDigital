using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Infraestructura.Transversal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato
{
    public interface INotarioServicio:IDisposable
    {
        Task<NotarioDTO> ActualizarGrafoPinNotario(NotarioCreateDTO Notario);
        Task<EstadoPinFirmaDTO> ObtenerEstadoPinFirma(string email);
        //Task<OpcionesConfiguracioNotarioDTO> ObtenerOpcionesConfiguracion(string email);
        Task<string> ObtenerGrafo(string email);
        //Task SeleccionarFormatoImpresion(SeleccionarFormatoImpresionDTO seleccionFormatoImpresionDTO);
        Task<IEnumerable<NotarioReturnDTO>> ObtenerNotariosNotaria(long NotariaId);
        Task<long> SeleccionarNotarioNotaria(NotarioNotariaDTO notarioNotariaDTO);
        Task<bool> ValidarSolicitudPin(ValSolicitudPinDTO valSolicitudPinDTO);
        Task<bool> EsPinValido(ValSolicitudPinDTO valSolicitudPinDTO);
    }
}
