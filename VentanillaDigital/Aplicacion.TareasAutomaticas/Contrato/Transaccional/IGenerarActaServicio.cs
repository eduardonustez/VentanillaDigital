using Dominio.ContextoPrincipal.Vista;
using System;
using System.Threading.Tasks;

namespace Aplicacion.TareasAutomaticas.Contrato.Transaccional
{
    public interface IGenerarActaServicio : IDisposable
    {
        Task<(string archivo, string tiempos)> GenerarArchivoPrevioAsync(
            long tramiteId,
            bool usarSticker = false,
            string notarioUser = null,
            string grafo = "",
            string sello = "",
            bool esFirmaManual = false,
            DatosActaFirmaManual datosActaFirmaManual = null);
    }
}