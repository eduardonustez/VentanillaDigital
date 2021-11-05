using ApiGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Notario
{

    public interface INotarioService
    {
        Task<bool> ConfigurarFirmaPin(string email, string pin, string grafo);

        Task<EstadoPinFirmaModel> ObtenerEstadoPinFirma(string email);
        Task<OpcionesConfiguracioNotarioModel> ObtenerOpcionesConfiguracion(string email);

        Task<string> ObtenerGrafo(string email);
        Task<bool> SeleccionarFormatoImpresion(bool SeleccionFormatoImpresion);
    }
}
