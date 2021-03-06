using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PortalCliente.Data;
using PortalCliente.Services;
using PortalCliente.Services.Biometria;
using PortalCliente.Services.Biometria.Models;
using PortalCliente.Services.Wacom;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PortalCliente.Services.Notaria;
using System.Net.Http;
using PortalCliente.Services.LoadingScreenService;
using Infraestructura.Transversal.Log.Modelo;
using Newtonsoft.Json;
using PortalCliente.Services.DescriptorCliente;
using System.Timers;
using Microsoft.Extensions.Configuration;
namespace PortalCliente.Components.RegistroTramite
{
    public partial class CapturaHuella : ComponentBase
    {
        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected IRNECService RNECService { get; set; }

        [Inject]
        protected ICustomHttpClient CustomHttpClient { get; set; }

        [Inject]
        protected INotariaService NotariaService { get; set; }

        [Inject]
        protected ITrazabilidadService trazabilidadService { get; set; }


        [Inject]
        NotificationService notificationService { get; set; }

        [Inject]
        LoadingScreenService LoadingScreenService { get; set; }

        [Inject]
        IDescriptorCliente DescriptorCliente { get; set; }

        [Inject]
        IConfiguration Configuration{get;set; }

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> authenticationStateTask { get; set; }

        [Parameter]
        public Compareciente Compareciente { get; set; }

        [Parameter]
        public EventCallback<Compareciente> ComparecienteChanged { get; set; }

        [Parameter]
        public long CodigoTipoTramite { get; set; }


        [Parameter]
        public EventCallback<bool> ReadyChanged { get; set; }

        [Parameter]
        public Tramite Tramite { get; set; }

        private int _motivo;

        int _dedoSolicitado1;
        int _dedoSolicitado2;
        string _imgDedoSolicitado1 = "images/CapturaHuellas/mano-izquierda/izquierda.PNG";
        string _imgDedoSolicitado2 = "images/CapturaHuellas/mano-derecha/derecha.PNG";
        string imgDedoSolicitado1Wacom = "images/Pad/wacom-validacion-biometrica.jpg";
        string imgDedoSolicitado2Wacom = "images/Pad/wacom-validacion-biometrica.jpg";
        string _lblDedoSolicitado1 = "";
        string _lblDedoSolicitado2 = "";
        string _dedo1Class = "";
        string _dedo2Class = "";
        string _claseCalidadHuella1 = "";
        string _claseCalidadHuella2 = "";
        List<ObjDedo> ListaDedos;
        bool _chkSinBiometriaIsDisabled = false;
        bool _capturandoHuella1 = false;
        bool _capturandoHuella2 = false;
        bool _okHuella1 = false;
        bool _okHuella2 = false;
        int _calidadHuella1 = 0;
        int _calidadHuella2 = 0;
        string strRespuestaHuellas = "Respuesta: Pendiente";
        string _userName;
        long _notariaId;
        int counter1;
        List<Dedo> _exceptuados;

        IDictionary<int, string> MotivosSinBiometria = new Dictionary<int, string>();

        bool _slSinBiometriaIsDisabled = false;
        int _slOpcionSelect = 0;
        bool _esMovil = false;
        private Timer _timer1;
        protected async override Task OnInitializedAsync()
        {
            var authState = await authenticationStateTask;
            var user = authState.User;
            _esMovil = await DescriptorCliente.EsMovil;
            _userName = user.Identity.Name;
            if (!long.TryParse(
                user.Claims.FirstOrDefault(c => c.Type == "NotariaId")?.Value, out _notariaId))
                _notariaId = -1;

            await FillMotivos();

            HuellasAleatorias();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            _exceptuados = new List<Dedo>();

            if (Compareciente?.Excepciones?.Dedos != null)
                _exceptuados.AddRange(Compareciente.Excepciones.Dedos);

            switch (Compareciente?.TipoIdentificacion.Abreviatura)
            {
                case "TI":
                    _motivo = _slOpcionSelect = 1;
                    _chkSinBiometriaIsDisabled = Compareciente.TramiteSinBiometria = _slSinBiometriaIsDisabled = true;
                    break;
                case "CE":
                case "P":
                    _motivo = _slOpcionSelect = 2;
                    _chkSinBiometriaIsDisabled = Compareciente.TramiteSinBiometria = _slSinBiometriaIsDisabled = true;
                    break;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (Compareciente.TramiteSinBiometria && _motivo > 0)
                {
                    await ReadyChanged.InvokeAsync(true);
                }

                if (_esMovil)
                {
                    await VerificarCaptor();
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        void ExcepcionesChanged(List<Dedo> exceptudados)
        {
            _exceptuados = exceptudados;
            if (Compareciente.Excepciones == null)
            {
                Compareciente.Excepciones = new Data.ExcepcionHuella();
            }
            Compareciente.Excepciones.Dedos = _exceptuados.ToArray();
            HuellasAleatorias();
            StateHasChanged();
        }


        private async Task Captura1()
        {
            if (_capturandoHuella1 || _capturandoHuella2)
                return;
            _dedo1Class = string.Empty;
            _capturandoHuella1 = true;
            InitializeCounter1();
            _calidadHuella1 = (await RNECService.Captura1((Dedo)_dedoSolicitado1)) * 100 / 170;
             FinalizeCounter1();
            _okHuella1 = _calidadHuella1 > 0;
            _claseCalidadHuella1 = ClaseColorPorCalidad(_calidadHuella1);
            _capturandoHuella1 = false;
            _dedo1Class = _okHuella1 ? "ok" : "warning";
            await ReadyChanged.InvokeAsync(_okHuella1 & _okHuella2);
        }

        private string ClaseColorPorCalidad(int calidad)
        {
            if (calidad == 0)
                return "gris";
            else if (calidad <= 45)
                return "rojo";
            else if (calidad <= 50)
                return "naranja";
            else if (calidad <= 60)
                return "amarillo";
            else
                return "verde";
        }
        private async Task Captura2()
        {
            if (_capturandoHuella2 || _capturandoHuella1)
                return;
            _dedo2Class = string.Empty;
            _capturandoHuella2 = true;
            InitializeCounter1();
            _calidadHuella2 = (await RNECService.Captura2((Dedo)_dedoSolicitado2)) * 100 / 170;
             FinalizeCounter1();
            _okHuella2 = _calidadHuella2 > 0;
            _claseCalidadHuella2 = ClaseColorPorCalidad(_calidadHuella2);
            _capturandoHuella2 = false;
            _dedo2Class = _okHuella2 ? "ok" : "warning";
            await ReadyChanged.InvokeAsync(_okHuella1 & _okHuella2);
        }
        private void InitializeCounter1()
        {
            if (_capturandoHuella1 || _capturandoHuella2)
            {
                counter1 = Configuration.GetValue<int>("WaitingTimeFingerPrintsInSeconds");
                _timer1 = new Timer(1000);
                _timer1.Start();
                _timer1.Elapsed += CountDownTimer1;
            }
        }
        private void FinalizeCounter1(){
            if(_timer1!=null && (_capturandoHuella1 || _capturandoHuella2)){
            _timer1.Stop();
            _timer1.Dispose();
            }
        }
        public void CountDownTimer1(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (counter1 > 0)
            {
                counter1 -= 1;
            }
            else
            {
                _timer1.Stop();
            }
            InvokeAsync(StateHasChanged);
        }
        public async Task Validar()
        {
            if (Compareciente.TramiteSinBiometria && _motivo > 0)
            {
                Compareciente.TransaccionRNECId = _motivo.ToString();
                Compareciente.MotivoSinBiometria = MotivosSinBiometria.FirstOrDefault(m => m.Key == _motivo).Value;
                Compareciente.ResulHuellas = "Tramite realizado sin biometría";
                return;
            }

            var notariaresponse = await NotariaService.ObtenerConfiguracionRNEC(_notariaId);
            await CustomHttpClient.GetJsonAsync<int>("Account/ValidateToken");

            LoadingScreenService.Show("Validando identidad con RNEC");
            var req = new ValidacionRequest()
            {
                Asesor = _userName,
                Oficina = notariaresponse.OficinaRNEC,
                Ciudadano = Compareciente.NumeroIdentificacion,
                ProductoId = CodigoTipoTramite,
                ConvenioId = notariaresponse.ConvenioRNEC,
                ClienteId = notariaresponse.ClienteRNECId,
                Grafo = string.IsNullOrWhiteSpace(Compareciente?.Firma) ? null : Regex.Replace(Compareciente?.Firma?.Replace("data:", ""), "^.+,", "")
            };
            int posicionActualCompa = (Compareciente?.Tramite?.Comparecientes?.Count() ?? 0) + 1;
            try
            {
                var resp = await RNECService.ValidarIdentidad(req);
                Compareciente.NombresRNEC = $"{resp.PrimerNombre} {resp.SegundoNombre}";
                Compareciente.ApellidosRNEC = $"{ resp.PrimerApellido} {resp.SegundoApellido}";
                Compareciente.TransaccionRNECId = resp.NutValidacion;
                Compareciente.ResulHuellas = resp.Validado ? $"HIT - {resp.Vigencia}" : "NO HIT";
                Compareciente.HuellasOk = resp.Validado;
                Compareciente.ResulHuellasDetalle = resp.Vigencia;
                Compareciente.DedoUnoResultado = (resp.Huellas != null ? resp.Huellas[0].Hit ? $"HIT" : "NO HIT" : "N/A");
                if (resp.Huellas.Length > 1)
                    Compareciente.DedoDosResultado = (resp.Huellas != null ? resp.Huellas[1].Hit ? $"HIT" : "NO HIT" : "N/A");
                Compareciente.ScoreDedoUnoResultado = (resp.Huellas != null ? resp.Huellas[0].Score : 0);
                Compareciente.ScoreDedoDosResultado = (resp.Huellas != null ? resp.Huellas[1].Score : 0);


                InformationModel objTraza = new InformationModel("1",
                   "Portal Cliente",
                   "CapturaHuella",
                   $"Validación RNEC - Compareciente {posicionActualCompa}",
                   "Tramite",
                   $"{Compareciente.Tramite.TramiteId}",
                   ObtenerComparecienteTraza(Compareciente, false), _userName);
                await trazabilidadService.CrearTraza(objTraza);
                await ComparecienteChanged.InvokeAsync(Compareciente);
            }
            catch (ApplicationException ex)
            {
                string mensaje = ex.Message == "Candidato no encontrado" ? "El Compareciente no fue encontrado en la Base de datos de la Registraduría. Revise el número de documento." : ex.Message;
                InformationModel objTraza = new InformationModel("1",
                   "Portal Cliente",
                   "CapturaHuella",
                   $"Validación RNEC - Compareciente {posicionActualCompa}",
                   "Tramite",
                   $"{Compareciente.Tramite.TramiteId}",
                   ObtenerComparecienteTraza(Compareciente, true, mensaje), _userName);
                await trazabilidadService.CrearTraza(objTraza);
                ShowErrorNotification(mensaje);
                throw;
            }
            catch (HttpRequestException ex)
            {
                ShowErrorNotification(ex.Message);
                InformationModel objTraza = new InformationModel("1",
                   "Portal Cliente",
                   "CapturaHuella",
                   $"Validación RNEC - Compareciente {posicionActualCompa}",
                   "Tramite",
                   $"{Compareciente.Tramite.TramiteId}",
                   ObtenerComparecienteTraza(Compareciente, false, ex.Message), _userName);
                await trazabilidadService.CrearTraza(objTraza);
                throw;
            }
            finally
            {
                LoadingScreenService.Hide();
            }
        }

        private async Task VerificarCaptor (bool reiniciar = true)
        {
            for (int intentos = 0; intentos < 15; intentos++)
            {
                try
                {
                    var estado = await RNECService.ConsultarEstado();
                    if (estado?.Propiedades != null &&
                        estado.Propiedades.Any(p => p.Key == "CaptorDetectado" && p.Value == true.ToString()))
                    {
                        await JSRuntime.InvokeVoidAsync("cambiarModal", "reconectar", "hide");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.ToString());
                }
                if (reiniciar)
                {
                    await ReiniciarCaptor();
                    return;
                }
                await Task.Delay(1000);
            }
            await JSRuntime.InvokeVoidAsync("cambiarModal", "reconectar", "hide");
        }

        private async Task ReiniciarCaptor()
        {
            await JSRuntime.InvokeVoidAsync("cambiarModal", "reconectar", "show");
            await RNECService.ReiniciarCaptor();
            await VerificarCaptor(false);
        }


        private string ObtenerComparecienteTraza(Compareciente compareciente, bool esError, string mensajeError = "")
        {
            if (esError)
            {
                var comparecienteTrazaError = new
                {
                    Nombre = compareciente.NombreCompleto ?? compareciente.NombreCompletoDigitado,
                    NumeroIdentificacion = compareciente.NumeroIdentificacion,
                    TipoIdentificacion = compareciente.TipoIdentificacion.TipoIdentificacionId,
                    Validado = compareciente.HuellasOk,
                    Error = mensajeError
                };
                return JsonConvert.SerializeObject(comparecienteTrazaError);
            }
            var comparecienteTraza = new
            {
                Nombre = compareciente.NombreCompleto ?? compareciente.NombreCompletoDigitado,
                NumeroIdentificacion = compareciente.NumeroIdentificacion,
                TipoIdentificacion = compareciente.TipoIdentificacion.TipoIdentificacionId,
                TransaccionRNECId = compareciente.TransaccionRNECId,
                Validado = compareciente.HuellasOk,
                Vigencia = compareciente.ResulHuellasDetalle,
                DedoUnoResultado = compareciente.DedoUnoResultado,
                DedoDosResultado = compareciente.DedoDosResultado,
                ScoreDedoUno = compareciente.ScoreDedoUnoResultado,
                ScoreDedoDos = compareciente.ScoreDedoDosResultado
            };
            string resul = JsonConvert.SerializeObject(comparecienteTraza);
            return resul;
        }
        private void HuellasAleatorias()
        {
            LlenarDedos();
            ObjDedo[] ConjuntoDedos1 =
                ListaDedos.Where(d => d.Mano == enumMano.Izquierda.ToString()).ToArray();
            ObjDedo[] ConjuntoDedos2 =
                ListaDedos.Where(d => d.Mano == enumMano.Derecha.ToString()).ToArray();
            int index = 0;
            Random rnd = new Random();
            if (ConjuntoDedos1.Length > 0 && ConjuntoDedos2.Length > 0)
            {

                index = rnd.Next(ConjuntoDedos1.Length);
                _dedoSolicitado1 = ConjuntoDedos1[index].NumDedoReconoser;
                _imgDedoSolicitado1 = ConjuntoDedos1[index].ImgDedo;
                _lblDedoSolicitado1 = ConjuntoDedos1[index].Titulo;
                imgDedoSolicitado1Wacom = ConjuntoDedos1[index].ImgDedoWacom;

                index = rnd.Next(ConjuntoDedos2.Length);
                _dedoSolicitado2 = ConjuntoDedos2[index].NumDedoReconoser;
                _imgDedoSolicitado2 = ConjuntoDedos2[index].ImgDedo;
                _lblDedoSolicitado2 = ConjuntoDedos2[index].Titulo;
                imgDedoSolicitado2Wacom = ConjuntoDedos2[index].ImgDedoWacom;
            }
            else if (ConjuntoDedos1.Length == 0 && ConjuntoDedos2.Length == 0)
            {
                Compareciente.TramiteSinBiometria = true;
            }
            else
            {
                var ListAux = new List<ObjDedo>();
                ListAux.AddRange(ConjuntoDedos1);
                ListAux.AddRange(ConjuntoDedos2);
                ObjDedo[] ConjuntoDedosAux = ListAux.ToArray();

                index = rnd.Next(ConjuntoDedosAux.Length);
                _dedoSolicitado1 = ConjuntoDedosAux[index].NumDedoReconoser;
                _imgDedoSolicitado1 = ConjuntoDedosAux[index].ImgDedo;
                _lblDedoSolicitado1 = ConjuntoDedosAux[index].Titulo;

                ConjuntoDedosAux = ConjuntoDedosAux.Where(d => d.NumDedoReconoser != _dedoSolicitado1).ToArray();
                index = rnd.Next(ConjuntoDedosAux.Length);
                _dedoSolicitado2 = ConjuntoDedosAux[index].NumDedoReconoser;
                _imgDedoSolicitado2 = ConjuntoDedosAux[index].ImgDedo;
                _lblDedoSolicitado2 = ConjuntoDedosAux[index].Titulo;
            }

        }


        void LlenarDedos()
        {
            ListaDedos = new List<ObjDedo>();
            ListaDedos.Add(new ObjDedo
            {
                Dedo = Dedo.PulgarDerecho,
                NumDedoReconoser = 1,
                ImgDedo = "images/CapturaHuellas/mano-derecha/derecha-1.png",
                Mano = enumMano.Derecha.ToString(),
                Titulo = "Pulgar - Mano Derecha",
                ImgDedoWacom = "images/Pad/mano-derecha/derecha-1.jpg"
            });
            ListaDedos.Add(new ObjDedo
            {
                Dedo = Dedo.IndiceDerecho,
                NumDedoReconoser = 2,
                ImgDedo = "images/CapturaHuellas/mano-derecha/derecha-2.png",
                Mano = enumMano.Derecha.ToString(),
                Titulo = "Índice - Mano Derecha",
                ImgDedoWacom = "images/Pad/mano-derecha/derecha-2.jpg"
            });
            ListaDedos.Add(new ObjDedo
            {
                Dedo = Dedo.MedioDerecho,
                NumDedoReconoser = 3,
                ImgDedo = "images/CapturaHuellas/mano-derecha/derecha-3.png",
                Mano = enumMano.Derecha.ToString(),
                Titulo = "Corazón - Mano Derecha",
                ImgDedoWacom = "images/Pad/mano-derecha/derecha-3.jpg"
            });
            ListaDedos.Add(new ObjDedo
            {
                Dedo = Dedo.AnularDerecho,
                NumDedoReconoser = 4,
                ImgDedo = "images/CapturaHuellas/mano-derecha/derecha-4.png",
                Mano = enumMano.Derecha.ToString(),
                Titulo = "Anular - Mano Derecha",
                ImgDedoWacom = "images/Pad/mano-derecha/derecha-4.jpg"
            });
            ListaDedos.Add(new ObjDedo
            {
                Dedo = Dedo.MeniqueDerecho,
                NumDedoReconoser = 5,
                ImgDedo = "images/CapturaHuellas/mano-derecha/derecha-5.png",
                Mano = enumMano.Derecha.ToString(),
                Titulo = "Meñique - Mano Derecha",
                ImgDedoWacom = "images/Pad/mano-derecha/derecha-5.jpg"
            });
            ListaDedos.Add(new ObjDedo
            {
                Dedo = Dedo.PulgarIzquierdo,
                NumDedoReconoser = 6,
                ImgDedo = "images/CapturaHuellas/mano-izquierda/izquierda-1.png",
                Mano = enumMano.Izquierda.ToString(),
                Titulo = "Pulgar - Mano Izquierda",
                ImgDedoWacom = "images/Pad/mano-izquierda/izquierda-1.jpg",
            });
            ListaDedos.Add(new ObjDedo
            {
                Dedo = Dedo.IndiceIzquierdo,
                NumDedoReconoser = 7,
                ImgDedo = "images/CapturaHuellas/mano-izquierda/izquierda-2.png",
                Mano = enumMano.Izquierda.ToString(),
                Titulo = "Índice - Mano Izquierda",
                ImgDedoWacom = "images/Pad/mano-izquierda/izquierda-2.jpg"
            });
            ListaDedos.Add(new ObjDedo
            {
                Dedo = Dedo.MedioIzquierdo,
                NumDedoReconoser = 8,
                ImgDedo = "images/CapturaHuellas/mano-izquierda/izquierda-3.png",
                Mano = enumMano.Izquierda.ToString(),
                Titulo = "Corazón - Mano Izquierda",
                ImgDedoWacom = "images/Pad/mano-izquierda/izquierda-3.jpg"
            });
            ListaDedos.Add(new ObjDedo
            {
                Dedo = Dedo.AnularIzquierdo,
                NumDedoReconoser = 9,
                ImgDedo = "images/CapturaHuellas/mano-izquierda/izquierda-4.png",
                Mano = enumMano.Izquierda.ToString(),
                Titulo = "Anular - Mano Izquierda",
                ImgDedoWacom = "images/Pad/mano-izquierda/izquierda-4.jpg",
            });
            ListaDedos.Add(new ObjDedo
            {
                Dedo = Dedo.MeniqueIzquierdo,
                NumDedoReconoser = 10,
                ImgDedo = "images/CapturaHuellas/mano-izquierda/izquierda-5.png",
                Mano = enumMano.Izquierda.ToString(),
                Titulo = "Meñique - Mano Izquierda",
                ImgDedoWacom = "images/Pad/mano-izquierda/izquierda-5.jpg",
            });

            var excepciones = Compareciente.Excepciones;
            if (excepciones != null && excepciones.Dedos?.Length > 0)
            {
                ListaDedos = ListaDedos.Where(d => !excepciones.Dedos.Contains(d.Dedo)).ToList();
            }
            else
            {
                ListaDedos = ListaDedos.Where(d => d.Dedo != Dedo.IndiceDerecho).ToList();
            }

        }
        async void ShowErrorNotification(string msg)
        {
            var message = new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Summary = "Error",
                Detail = $"{msg}",
                Duration = 4000
            };
            notificationService.Notify(message);
        }

        async Task FillMotivos()
        {
            var mayoresDeEdad = new List<string> { "CC", "CE" };
            var extranjero = new List<string> { "P", "CE" };
            if (!mayoresDeEdad.Contains(Compareciente?.TipoIdentificacion.Abreviatura))
            {
                MotivosSinBiometria.Add(1, "Menor de edad");
            }
            if (extranjero.Contains(Compareciente?.TipoIdentificacion.Abreviatura))
            {
                MotivosSinBiometria.Add(2, "Extranjero (Pasaporte - Cédula de extranjería)");
            }
            MotivosSinBiometria.Add(3, "Imposibilidad de captura de huellas");
            MotivosSinBiometria.Add(4, "Fallas de conectividad");
            MotivosSinBiometria.Add(5, "Otras excepciones de ley");
            MotivosSinBiometria.Add(6, "Otro");
        }
        // private async Task InformarCapturaHuellasWacom(string imgDedoSolicitadoWacom = "")
        // {
        //     await WacomService.VerificarServicio(2);

        //     if (!_esMovil && WacomService.EstadoServicio == Services.Wacom.Models.EstadoServicio.DispositivoConectado)
        //     {
        //         if (string.IsNullOrWhiteSpace(imgDedoSolicitadoWacom))
        //             await WacomService.LimpiarPantalla();
        //         else
        //             await WacomService.MostrarImagen(imgDedoSolicitadoWacom);
        //     }
        // }

        private class ObjDedo
        {
            public Dedo Dedo { get; set; }
            public int NumDedoReconoser { get; set; }
            public string ImgDedo { get; set; }
            public string ImgDedoWacom { get; set; }
            public string Mano { get; set; }
            public string Titulo { get; set; }
        }

        private enum enumMano
        {
            Izquierda = 1,
            Derecha = 2
        }

    }


}
