using PortalAdministrador.Services.Wacom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace PortalAdministrador.Services.Wacom
{
    public interface IWacomService
    {
        public event EventHandler<EstadoServicio> EstadoServicioChanged;
        public string FondoInactivo { get; set; }
        public string FondoActivo { get; }
        public bool Conectado { get; }
        public EstadoServicio EstadoServicio { get; }
        public Task VerificarServicio();
        public Task LimpiarPantalla();
        public Task IniciarCaptura(string fondo, Boton[] botones, bool firmando);
        public Task TerminarCaptura(string siguiente = null, Boton[] botones = null, bool firmando = false);
        public Task MostrarImagen(string fondo);
        public Task<string> ObtenerFirma(int alto, int ancho, Rectangle area);
    }
}
