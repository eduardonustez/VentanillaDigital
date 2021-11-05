using PortalCliente.Services.Wacom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace PortalCliente.Services.Wacom
{
    public interface IWacomService
    {
        public event EventHandler<EstadoServicio> EstadoServicioChanged;
        public string FondoInactivo { get; set; }
        public string FondoActivo { get; }
        public bool Conectado { get; }
        public EstadoServicio EstadoServicio { get; }
        Task Initialize(bool pantallaInicio=true);
        public Task VerificarServicio();
        public Task LimpiarPantalla();
        public Task IniciarCaptura(string fondo, Boton[] botones, bool firmando,SignatureSize tamanioFirma = null);
        public Task TerminarCaptura(string siguiente = null, Boton[] botones = null, bool firmando = false);
        public Task MostrarImagen(string fondo);
        public Task<string> ObtenerFirma(int alto, int ancho, double grosor);
    }
}
