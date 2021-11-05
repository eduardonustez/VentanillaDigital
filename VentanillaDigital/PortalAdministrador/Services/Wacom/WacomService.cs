using Microsoft.JSInterop;
using PortalAdministrador.Services.Wacom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortalAdministrador.Services.Recursos;
using System.Drawing;

namespace PortalAdministrador.Services.Wacom
{
    public class WacomService : IWacomService, IDisposable
    {
        private IJSRuntime JSRuntime;
        public readonly IRecursosService RecursosService;
        private DotNetObjectReference<WacomService> objRef;

        public WacomService (IJSRuntime jSRuntime, IRecursosService recursosService)
        {
            JSRuntime = jSRuntime;
            RecursosService = recursosService;
            objRef = DotNetObjectReference.Create(this);
        }


        public async Task Initialize()
        {
            EstadoServicio = EstadoServicio.Verificando;
            await JSRuntime.InvokeVoidAsync("setHelper", objRef);
            await JSRuntime.InvokeVoidAsync("onLoad");
            await VerificarServicio();


            if (EstadoServicio == EstadoServicio.DispositivoConectado)
            {
                await LimpiarPantalla();
            }
        }

        #region JSInvokable
        [JSInvokable]
        public async Task ActualizarEstadoServicio (bool conectado)
        {
            Conectado = conectado;
        }

        [JSInvokable]
        public async Task ActivarBoton(int boton)
        {
            if (boton < Botones?.Length &&
                Botones[boton]?.Accion != null)
                await Botones[boton].Accion();
        }
        #endregion

        #region Public
        public string FondoInactivo { get; set; }
        public string FondoActivo { get; private set; }
        private EstadoServicio _estadoServicio;
        public EstadoServicio EstadoServicio
        {
            get
            {
                return _estadoServicio; 
            }
            private set
            {
                _estadoServicio = value;
                EstadoServicioChanged?.Invoke(this, _estadoServicio);
            }
        }

        public async Task VerificarServicio()
        {
            EstadoServicio = EstadoServicio.Verificando;
            for (int i = 0; i < 20; i++)
            {
                EstadoServicio = (EstadoServicio)await JSRuntime.InvokeAsync<int>("checkArbitratorConnection");
                if (EstadoServicio == EstadoServicio.DispositivoConectado)
                    break;
                await Task.Delay(1000);
            }
        }

        public async Task LimpiarPantalla()
        {
            var conectado = Conectado;

            if (!conectado)
            {
                await JSRuntime.InvokeVoidAsync("connect");
            }
            if(Capturando)
            {
                await EnviarImagen(FondoActivo);
            }
            else if (!string.IsNullOrEmpty(FondoInactivo))
            {
                await EstablecerInput(false);
                await EnviarImagen(FondoInactivo);
            }
            else
            {
                await EstablecerInput(false);
                await JSRuntime.InvokeVoidAsync("ClearSTU");
            }
            if(!conectado)
            {
                await JSRuntime.InvokeVoidAsync("disconnect");
            }
        }

        public async Task IniciarCaptura(string fondo, Boton[] botones, bool firmando)
        {
            var conectado = Conectado;
            if (!conectado)
            {
                await JSRuntime.InvokeVoidAsync("connect");
            }
            await EstablecerInput(false);
            FondoActivo = fondo;
            Botones = botones;
            Capturando = true;
            await EnviarImagen(fondo);
            await EstablecerEscucha(botones.Select(x=>x.Posicion).ToArray(), firmando, new Size(800,480));
        }

        public async Task MostrarImagen(string fondo)
        {
            var conectado = Conectado;
            if (!conectado)
            {
                await JSRuntime.InvokeVoidAsync("connect");
            }

            Capturando = false;
            await EstablecerInput(false);
            await EnviarImagen(fondo);

            if (!conectado)
            {
                await JSRuntime.InvokeVoidAsync("disconnect");
            }
        }

        public async Task<string> ObtenerFirma(int alto, int ancho, Rectangle area)
        {
            return await JSRuntime.InvokeAsync<string> ("generateImage", alto, ancho, area);
        }

        public async Task TerminarCaptura(string siguiente=null, Boton[] botones = null, bool firmando = false)
        {
            Botones = botones;
            if (siguiente != null && (botones != null || firmando))
            {
                await IniciarCaptura(siguiente, Botones, firmando);
            }
            else
            {
                Capturando = false;
                FondoActivo = null;
                Botones = null;
                if (siguiente == null)
                    await LimpiarPantalla();
                else
                    await MostrarImagen(siguiente);
                await Cerrar();
            }
        }
        #endregion

        #region private
        private Boton[] Botones { get; set; }
        private bool Capturando { get; set; }
        public bool Conectado { get; private set; }

        public event EventHandler<EstadoServicio> EstadoServicioChanged;

        private async Task Cerrar()
        {
            await JSRuntime.InvokeVoidAsync("disconnect");
        }

        private async Task EnviarImagen(string recurso)
        {
            bool conectado = Conectado;
            if(!conectado)
            {
                await JSRuntime.InvokeVoidAsync("connect");
            }
            var data = await RecursosService.ObtenerRecurso(recurso);
            await JSRuntime.InvokeVoidAsync("SendImgToSTU",data);
            if(!conectado)
            {
                await JSRuntime.InvokeVoidAsync("disconnect");
            }
        }

        private async Task EstablecerEscucha(Rectangle[] botones, bool firmando, Size tamano)
        {
            bool conectado = Conectado;
            if (!conectado)
            {
                await JSRuntime.InvokeVoidAsync("connect");
            }
            await JSRuntime.InvokeVoidAsync("SetListeningMode", botones, firmando, tamano);
        }

        private async Task EstablecerInput(bool activado)
        {
            bool conectado = Conectado;
            if (!conectado)
            {
                await JSRuntime.InvokeVoidAsync("connect");
            }
            await JSRuntime.InvokeVoidAsync("SetInkingMode", activado?0x01:0x00);
            if (!conectado)
            {
                await JSRuntime.InvokeVoidAsync("disconnect");
            }
        }

        public async void Dispose()
        {
            if (Conectado)
                await JSRuntime.InvokeVoidAsync("disconnect");
            objRef?.Dispose();
        }

        #endregion
    }
}
