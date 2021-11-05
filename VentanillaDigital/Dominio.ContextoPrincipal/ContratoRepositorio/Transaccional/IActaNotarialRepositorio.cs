using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.ContextoPrincipal.Vista;
using Dominio.Nucleo;
using Infraestructura.Transversal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional
{
    public interface IActaNotarialRepositorio : IRepositorioBase<MetadataArchivo>, IDisposable
    {
        string[] ObtenerCamposActaNotarial(long TramiteId, string notarioUser);
        Task<InfoActa> ObtenerInformacionPrincipalActaNotarial(long tramiteId, string notarioUser);
        Task<IEnumerable<ComparecienteData>> ObtenerComparecientes(long tramiteid);
        Task<Parametrica> ObtenerParametrica(string valor);
        Task<DatosNotarioFirmaManual> ObtenerDatosNotarioFirmaManual(long notariaId);
        Task<ArchivoActa> ObtenerActa(long tramiteId);
        Task<ArchivoActa> ObtenerActa(long tramiteId, long notariaId, DateTime fecha);
    }
}
