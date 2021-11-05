using Microsoft.JSInterop;
using PortalCliente.Services.Wacom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortalCliente.Services.Recursos;
using System.Drawing;
using Newtonsoft.Json;
namespace PortalCliente.Services.Wacom
{
    public class WacomAgenteService : IWacomService, IDisposable
    {
        private IJSRuntime JSRuntime;
        public readonly IRecursosService RecursosService;
        private DotNetObjectReference<WacomAgenteService> objRef_WacomAgenteService;

        public WacomAgenteService(IJSRuntime jSRuntime, IRecursosService recursosService)
        {
            JSRuntime = jSRuntime;
            RecursosService = recursosService;
            objRef_WacomAgenteService = DotNetObjectReference.Create(this);
        }


        public async Task Initialize(bool pantallaInicio = true)
        {
            EstadoServicio = EstadoServicio.Verificando;
            await JSRuntime.InvokeVoidAsync("SR_SetHelper", objRef_WacomAgenteService);
            //await JSRuntime.InvokeVoidAsync("SR_connect");
            //await ConnectSignalR();
            await VerificarServicio();
            // if (EstadoServicio == EstadoServicio.DispositivoConectado)
            // {
            //     await LimpiarPantalla();
            // }
            if (pantallaInicio)
                await LimpiarPantalla();

        }

        #region JSInvokable
        [JSInvokable]
        public async Task SR_ActualizarEstadoServicio(bool conectado)
        {
            Conectado = conectado;
        }

        [JSInvokable]
        public async Task SR_ActivarBoton(int boton)
        {
            if (boton < Botones?.Length &&
                Botones[boton]?.Accion != null)
            {
                await Botones[boton].Accion();
            }

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
            // EstadoServicio = EstadoServicio.Verificando;
            // for (int i = 0; i < 20; i++)
            // {
            //     bool isConnectedWacom= await JSRuntime.InvokeAsync<bool>("SR_IsConnectedWacom");
            //     EstadoServicio = isConnectedWacom?EstadoServicio.DispositivoConectado:EstadoServicio.Desconectado;
            //     if (EstadoServicio == EstadoServicio.DispositivoConectado)
            //         break;
            //     await Task.Delay(1000);
            // }
            EstadoServicio = EstadoServicio.DispositivoConectado;
        }
        public async Task VerificarServicio(int intentos)
        {
            EstadoServicio = EstadoServicio.Verificando;
            for (int i = 0; i < intentos; i++)
            {
                bool isConnectedWacom = await JSRuntime.InvokeAsync<bool>("SR_IsConnectedWacom");
                EstadoServicio = isConnectedWacom ? EstadoServicio.DispositivoConectado : EstadoServicio.Desconectado;
                if (EstadoServicio == EstadoServicio.DispositivoConectado)
                    break;
                await Task.Delay(1000);
            }
        }

        public async Task LimpiarPantalla()
        {
            if (Capturando)
            {
                await EstablecerInput(true);
                await EnviarImagen(FondoActivo,true);
            }
            else if (!string.IsNullOrEmpty(FondoInactivo))
            {
                await EstablecerInput(false);
                await EnviarImagen(FondoInactivo);
            }
            else
            {
                await EstablecerInput(false);
                //await JSRuntime.InvokeVoidAsync("ClearSTU");
            }
        }

        public async Task IniciarCaptura(string fondo, Boton[] botones, bool firmando, SignatureSize tamanioFirma = null)
        {
            // Console.WriteLine("Iniciar captura wacom signalR 🚖🚖🚖");
            // var conectado = Conectado;
            // if (!conectado)
            // {
            //     await JSRuntime.InvokeVoidAsync("SR_ConnectWacom");
            // }
            await EstablecerInput(firmando);
            FondoActivo = fondo;
            Botones = botones;
            Capturando = true;
            await EnviarImagen(fondo);
            await EstablecerEscucha(botones.Select(x => x.Posicion
             ).ToArray(), firmando, new Size(800, 480));
            if(tamanioFirma!=null){
                await JSRuntime.InvokeVoidAsync("SR_SetCapturedImageSize", tamanioFirma.Height,
                    tamanioFirma.Width, tamanioFirma.LineWidth);
            }
        }

        public async Task MostrarImagen(string fondo)
        {
            await EstablecerInput(false);
            await EnviarImagen(fondo);
        }

        public async Task<string> ObtenerFirma(int alto, int ancho, double grosor)
        {
            return await JSRuntime.InvokeAsync<string>("SR_GenerateImage", alto, ancho, grosor);
        }

        public async Task TerminarCaptura(string siguiente = null, Boton[] botones = null, bool firmando = false)
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
                //await Cerrar();
            }
        }
        #endregion

        #region private
        private Boton[] Botones { get; set; }
        private bool Capturando { get; set; }
        public bool Conectado { get; private set; }

        public event EventHandler<EstadoServicio> EstadoServicioChanged;

        private async Task EnviarImagen(string recurso,bool force=false)
        {
            bool conectado = Conectado;
            if (!conectado)
            {
                //await JSRuntime.InvokeVoidAsync("connect");
            }
            string keyImage = recurso.GetHashCode().ToString();
            bool exists = await JSRuntime.InvokeAsync<bool>("SR_ImageExist", keyImage);
            if (!exists)
            {
                var data = await RecursosService.ObtenerRecurso(recurso);
                data = data.Replace("data:image/jpeg;base64,", "");
                await JSRuntime.InvokeVoidAsync("SR_AddImageToCache", keyImage, Convert.FromBase64String(data));
            }
            await JSRuntime.InvokeVoidAsync("SR_SendImgToSTU", keyImage,force);
            //var data = await RecursosService.ObtenerRecurso(recurso);
            //data = data.Replace("data:image/jpeg;base64,", "");
            //await JSRuntime.InvokeVoidAsync("SR_SendImgToSTU", JsonConvert.SerializeObject(SegmentarImagen(Convert.FromBase64String(data))));
            if (!conectado)
            {
                //await JSRuntime.InvokeVoidAsync("disconnect");
            }
        }
        private async Task<ImageChunk[]> SegmentarImagen(byte[] bytes)
        {
            List<ImageChunk> ImageBytes = new List<ImageChunk>();
            int chunkSize = Math.Min(bytes.Length, 20000);
            Guid id = Guid.NewGuid();
            for (int i = 0; i < bytes.Length; i += chunkSize)
            {
                chunkSize = Math.Min(bytes.Length - i, chunkSize);
                var chunk = new byte[chunkSize];
                //Array.Copy(bytes,i,chunk,0,chunk.Length);
                Buffer.BlockCopy(bytes, i, chunk, 0, chunk.Length);
                ImageBytes.Add(
                    new ImageChunk()
                    {
                        Id = id,
                        Bytes = chunk,
                        IsLast = bytes.Length - i <= chunkSize
                    }
                );
            }
            return ImageBytes.ToArray();
        }

        private async Task EstablecerEscucha(Rectangle[] botones, bool firmando, Size tamano)
        {
            
            await JSRuntime.InvokeVoidAsync("SR_AddButtons", JsonConvert.SerializeObject(botones), firmando);
        }

        private async Task EstablecerInput(bool activado)
        {
            bool conectado = Conectado;
            if (!conectado)
            {
                //await JSRuntime.InvokeVoidAsync("connect");
            }
            await JSRuntime.InvokeVoidAsync("SR_SetInkingMode", activado ? 0x01 : 0x00);
            if (!conectado)
            {
                //await JSRuntime.InvokeVoidAsync("disconnect");
            }
        }

        public async void Dispose()
        {
            if (Conectado)
                await JSRuntime.InvokeVoidAsync("disconnect");
            //objRef?.Dispose();
        }
        #endregion
        private class ImageChunk
        {
            public Guid Id { get; set; }
            public byte[] Bytes { get; set; }
            public bool IsLast { get; set; }
        }
    }
}
