using ApiGateway.Contratos.Models.Notario;
using BlazorInputFile;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using PortalCliente.Components.Transversales;
using PortalCliente.Functions;
using PortalCliente.Services;
using PortalCliente.Services.DescriptorCliente;
using PortalCliente.Services.LoadingScreenService;
using Radzen;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Pages.TramitePages
{
    public class TramiteFrm
    {
        public int? ActoPrincipalId { get; set; }
        public string ActoPrincipalNombre { get; set; }
    }

    public partial class TramitesPortalVirtualPageAutorizar : ComponentBase
    {
        #region Parameters and Injects
        [Parameter] public long tramiteId { get; set; }

        [Inject]
        protected IDescriptorCliente DescriptorCliente { get; set; }

        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }

        [Inject]
        private NotificationService notificationService { get; set; }

        [Inject]
        public ITramiteVirtualService tramitesVirtualService { get; set; }

        [Inject]
        public LoadingScreenService LoadingScreenService { get; set; }

        [Inject]
        public IConfiguration Configuration { get; set; }
        [Inject]
        IJSRuntime Js { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }
        #endregion

        #region Properties And Variables

        public TramiteVirtualModel Tramite { get; set; }
        public List<ArchivoTramiteVirtual> Archivos { get; set; } = new List<ArchivoTramiteVirtual>();
        public List<ActoPorTramiteModel> ActosPorTramite { get; set; } = new List<ActoPorTramiteModel>();
        public long ArchivoSeleccionadoId { get; set; }
        public int estadoSeleccionado { get; set; }
        private bool _movil = false;
        public string MensajeError { get; set; }
        public string MensajeAutorizacion { get; set; }
        public decimal Precio { get; set; }
        public string Razon { get; set; }
        public List<UploadFileModel> Files { get; set; } = new List<UploadFileModel>();
        public bool IsLoading { get; set; }
        public bool ShowDatosAdicionales { get; set; }
        ModalForm ModalDatosAdicionales { get; set; }
        Components.TransaccionesVirtuales.ModalFormVirtual ModalIframe { get; set; }
        ModalForm ModalShowPdf { get; set; }
        ModalForm ModalPagos { get; set; }
        ModalQuestion ModalQuestion { get; set; }
        ModalQuestion ModalQuestionRechazar { get; set; }
        ModalQuestion ModalQuestionAprobar { get; set; }
        AuthenticationState context { get; set; }
        public string ClaveTestamento { get; set; }
        public string UrlSubirArchivos { get; set; }
        public Coordenadas Coordenadas { get; set; }
        //public bool iframeLoading { get; set; }
        public string AuthToken { get; set; }
        public TramiteFrm TramiteFrm { get; set; } = new TramiteFrm();
        public List<ActoNotarialModel> ActosNotarialesLista { get; set; }
        public List<DatosAdicionalesCierre> DatosAdicionales { get; set; } = new List<DatosAdicionalesCierre>
        {
            new DatosAdicionalesCierre { Titulo = "Escritura", Field = "matricula" },
            new DatosAdicionalesCierre { Titulo = "Derechos Notariales", Field = "derechos_notariales" },
            new DatosAdicionalesCierre { Titulo = "Superintendencia de Notariado", Field = "superintendencia_notariado" },
            new DatosAdicionalesCierre { Titulo = "Fondo Nacional del Notariado", Field = "fondo_nacional_notariado" },
            new DatosAdicionalesCierre { Titulo = "RETEFUENTE", Field = "retefuente" },
            new DatosAdicionalesCierre { Titulo = "IVA", Field = "iva" }
        };
        public CultureInfo Culture { get { return CultureInfo.CreateSpecificCulture("es-CO"); } }
        public string Base64File { get; set; }
        public string NameFile { get; set; }
        public string MimeTypeFile { get; set; }
        public decimal TotalPagado { get; set; }
        private string TituloMinutaSubida;
        private string CUANDI;
        List<ComparecientesIFrame> comparecientesIFrame;
        #endregion

        /// <summary>
        /// Inicio del componenten
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            context = await _authenticationStateProvider.GetAuthenticationStateAsync();
            LoadingScreenService.ShowEvent += OnShowLoading;
            LoadingScreenService.HideEvent += OnHideLoading;
            _movil = await DescriptorCliente.EsMovil;
            IsLoading = true;
            AuthToken = await ((CustomAuthenticationStateProvider)_authenticationStateProvider).GetTokenFromAuthenticatedUser();
            await ObtenerActosNotariales();
            await ConsultarTramite(tramiteId);
            TotalPagado = await tramitesVirtualService.ObtenerTotalPagadoTramite((int)tramiteId);
            MensajeAutorizacion = $"Se ha recibido {TotalPagado.ToString("C0", Culture)} del trámite";
        }

        /// <summary>
        /// Solicita y obtiene los permisos de localización
        /// </summary>
        /// <returns></returns>
        private async Task ConsultarLocalizacion()
        {
            try
            {
                var res = await Js.InvokeAsync<object>("obtenerGeolocalizacion");
                Console.WriteLine($"Posicion: {res}");

                var localization = JsonConvert.DeserializeObject<Dictionary<string, string>>(res.ToString());

                if (localization != null)
                {
                    Coordenadas = new Coordenadas
                    {
                        Lat = decimal.Parse(localization["lat"].Replace(",", "."), CultureInfo.InvariantCulture),
                        Lng = decimal.Parse(localization["lng"].Replace(",", "."), CultureInfo.InvariantCulture),
                    };
                }
            }
            catch (Exception ex)
            {
                MensajeError = ex.Message;
            }
        }

        /// <summary>
        /// Evento show del loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShowLoading(object sender, string e)
        {
            IsLoading = true;
            StateHasChanged();
        }

        /// <summary>
        /// Evento hide del loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHideLoading(object sender, EventArgs e)
        {
            IsLoading = false;
            StateHasChanged();
        }

        #region Overrides

        #endregion

        #region Methods
        /// <summary>
        /// Obtiene los actos notariales
        /// </summary>
        /// <returns></returns>
        public async Task ObtenerActosNotariales()
        {
            ActosNotarialesLista = (await tramitesVirtualService.ObtenerActosNotariales())?.ToList();
        }

        /// <summary>
        /// Obtiene todos los actos por trámite
        /// </summary>
        /// <param name="tramiteId"></param>
        /// <returns></returns>
        public async Task ObtenerActosPorTramite(long tramiteId)
        {
            ActosPorTramite = (await tramitesVirtualService.ObtenerActosPorTramite(tramiteId))?.ToList();
        }

        /// <summary>
        /// Consulta el trámite
        /// </summary>
        /// <param name="tramiteId"></param>
        /// <returns></returns>
        async Task ConsultarTramite(long tramiteId)
        {
            var res = await tramitesVirtualService.ConsultarTramiteVirtualPorId(tramiteId);

            if (res != null)
            {
                TituloMinutaSubida = $"{res.TipoTramiteVirtualNombre} - {res.CUANDI}";
                CUANDI = res.CUANDI;

                await CargarDatosTramite(res);
                comparecientesIFrame = ObtenerDatosAdicionales();
            }
            else NavigationManager.NavigateTo($"/tramitesportalvirtual/");
        }

        private List<ComparecientesIFrame> ObtenerDatosAdicionales()
        {
            comparecientesIFrame = new List<ComparecientesIFrame>();
            if (Tramite.HasDatosAdicionales)
            {
                var converted = Newtonsoft.Json.JsonConvert.DeserializeObject(Tramite.DatosAdicionales);

                if (converted is Newtonsoft.Json.Linq.JObject keyValues)
                {
                    if (keyValues.ContainsKey("compareciente"))
                    {
                        var comparecientes = keyValues.SelectToken("compareciente");
                        ComparecientesIFrame compareciente = new ComparecientesIFrame();
                        if (comparecientes.SelectToken("personaCorreo") != null) compareciente.Correo = comparecientes.SelectToken("personaCorreo").ToString();
                        if (comparecientes.SelectToken("personaNombrecompleto") != null) compareciente.Nombre = $"{comparecientes.SelectToken("personaNombrecompleto")}";
                        else
                        {
                            if (comparecientes.SelectToken("personaPrimerNombre") != null)
                                compareciente.Nombre = $"{comparecientes.SelectToken("personaPrimerNombre")} {comparecientes.SelectToken("personaSegundoNombre")} {comparecientes.SelectToken("personaPrimerApellido")} {comparecientes.SelectToken("personaSegundoApellido")}";
                        }
                        if (!string.IsNullOrEmpty(compareciente.Nombre)
                            && !string.IsNullOrEmpty(compareciente.Correo))
                        {
                            comparecientesIFrame.Add(compareciente);
                        }
                    }
                }

                if (converted is Newtonsoft.Json.Linq.JArray datosAdicionalesArr)
                {
                    foreach (Newtonsoft.Json.Linq.JObject comparecienteObj in datosAdicionalesArr)
                    {
                        ComparecientesIFrame compareciente = new ComparecientesIFrame();
                        if (comparecienteObj.ContainsKey("FullName"))
                        {
                            compareciente.Nombre = comparecienteObj.SelectToken("FullName").ToString();
                        }
                        if (comparecienteObj.ContainsKey("Email"))
                        {
                            compareciente.Correo = comparecienteObj.SelectToken("Email").ToString();
                        }
                        if (!string.IsNullOrEmpty(compareciente.Nombre)
                            && !string.IsNullOrEmpty(compareciente.Correo))
                        {
                            comparecientesIFrame.Add(compareciente);
                        }
                    }
                }
            }
            return comparecientesIFrame;
        }

        /// <summary>
        /// Carga los datos del trámite
        /// </summary>
        /// <param name="tramite"></param>
        /// <returns></returns>
        async Task CargarDatosTramite(TramiteVirtualModel tramite)
        {
            Tramite = tramite;
            Archivos = tramite.Archivos;
            TramiteFrm.ActoPrincipalId = tramite.ActoPrincipalId;
            TramiteFrm.ActoPrincipalNombre = tramite.ActoPrincipalNombre;

            await ObtenerActosPorTramite(tramiteId);

            if (Tramite.EstadoTramiteVirtualId == 9) await ConsultarLocalizacion();
        }

        /// <summary>
        /// Muestra el archivo
        /// </summary>
        /// <param name="archivoId"></param>
        async Task ShowFile(long archivoId)
        {
            IsLoading = true;
            ArchivoSeleccionadoId = archivoId;
            await GetUrlPdf();
            ModalShowPdf.Open();
            StateHasChanged();
        }

        /// <summary>
        /// Obtiene la url para cargarlo
        /// </summary>
        /// <returns></returns>
        async Task GetUrlPdf()
        {
            var resultado = await tramitesVirtualService.ConsultarArchivoTramiteVirtualPorId(ArchivoSeleccionadoId, AuthToken);
            Base64File = resultado.Base64;
            NameFile = resultado.Nombre;
            MimeTypeFile = resultado.Formato;
            IsLoading = false;
        }

        /// <summary>
        /// Descarga el archivo
        /// </summary>
        /// <param name="archivoId"></param>
        /// <returns></returns>
        async Task DownloadFile(long archivoId)
        {
            ArchivoSeleccionadoId = archivoId;
            await GetUrlPdf();
            StateHasChanged();
            await Js.InvokeAsync<object>("saveAsFile", NameFile, Base64File);
        }

        /// <summary>
        /// Cambia de estado un trámite
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        async Task CambiarEstado(int estado)
        {
            try
            {
                MensajeError = string.Empty;
                if (estado.Equals(0))
                {
                    ShowWarningNotification("Advertencia", "Por favor seleccione un 'Estado'.");
                    return;
                }
                if (Tramite.EstadoTramiteVirtualId == 9 && Tramite.TipoTramiteVirtualId == 3)
                {
                    if (estado != 15)
                    {
                        if (DatosAdicionales.Any(m => string.IsNullOrEmpty(m.Value)))
                        {
                            MensajeError = "Por favor complete la información";
                            return;
                        }
                    }
                }

                var res = await tramitesVirtualService.CambiarEstado(
                    tramiteId,
                    estado,
                    Precio,
                    Razon,
                    Files,
                    ClaveTestamento,
                    Coordenadas,
                    TramiteFrm.ActoPrincipalId,
                    ActosPorTramite,
                    Tramite.EstadoTramiteVirtualId == (int)EstadoTramiteVirtual.PendienteAutorizar &&
                    (Tramite.TipoTramiteVirtualId == (int)TipoTramiteVirtual.Compraventa
                    || Tramite.TipoTramiteVirtualId == (int)TipoTramiteVirtual.Matrimonio
                    || Tramite.TipoTramiteVirtualId == (int)TipoTramiteVirtual.TestamentoAbierto
                    || Tramite.TipoTramiteVirtualId == (int)TipoTramiteVirtual.TestamentoCerrado
                    ) ?
                        JsonConvert.SerializeObject(new Dictionary<string, string>(
                            DatosAdicionales.Select(m => new KeyValuePair<string, string>(m.Field, m.Value)).ToList()
                            )
                        ) : ""
                );

                if (res != null)
                {
                    estadoSeleccionado = 0;
                    ShowSuccessNotification("Cambio de estado Exitoso", "El trámite ha cambiado de estado exitosamente.");

                    if (res != null) await CargarDatosTramite(res);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} 🤞🤞🤞");
                MensajeError = ex.Message;
            }

            StateHasChanged();
        }

        /// <summary>
        /// Agrega un nuevo acto notarial
        /// </summary>
        void NuevoActoNotarial()
        {
            ActosPorTramite.Add(new ActoPorTramiteModel { Cuandi = "0000000000001" });
        }

        void MostrarModalPagos()
        {
            ModalPagos.Open();
        }

        /// <summary>
        /// Elimina un acto notarial
        /// </summary>
        /// <param name="actoNotarialId"></param>
        void EliminarActoNotarial(int actoNotarialId)
        {
            if (ActosPorTramite.Any(m => m.ActoNotarialId == actoNotarialId))
            {
                ActosPorTramite.RemoveAll(m => m.ActoNotarialId == actoNotarialId);
                StateHasChanged();
            }
        }

        /// <summary>
        /// verifica cual pestaña se debe seleccionar
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        bool IsSelected(ArchivoTramiteVirtual archivo)
        {
            return archivo.ArchivosPortalVirtualId == ArchivoSeleccionadoId;
        }

        /// <summary>
        /// Obtiene el tipo para el documento a mostrar
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        string GetFileType(string formato) => formato switch
        {
            var x when x == "PDF" || x == "application/pdf" => "application/pdf",
            var x when x == "PNG" || x == "image/jpeg" => "image/jpeg",
            _ => formato
        };

        /// <summary>
        /// Mensaje de notificación
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        void ShowSuccessNotification(string title, string text)
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = title, Detail = text, Duration = 4000 };
            notificationService.Notify(message);
        }

        void ShowWarningNotification(string title, string text)
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Warning, Summary = title, Detail = text, Duration = 4000 };
            notificationService.Notify(message);
        }

        /// <summary>
        /// Decarga el testamento
        /// </summary>
        /// <returns></returns>
        async Task DownloadTestamento()
        {
            try
            {
                MensajeError = string.Empty;

                if (string.IsNullOrEmpty(ClaveTestamento))
                {
                    MensajeError = "Debe digitar la clave del testamento.";
                    return;
                }

                var res = await tramitesVirtualService.DescargarTestamento(tramiteId, ClaveTestamento);

                if (res != null)
                {
                    DateTime Jan1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    TimeSpan javaSpan = DateTime.UtcNow - Jan1970;
                    await saveAsFile($"Testamento_{javaSpan.TotalMilliseconds}.pdf", res.Base64);
                }
            }
            catch (System.Exception ex)
            {
                MensajeError = ex.Message;
            }

            StateHasChanged();
        }

        /// <summary>
        /// Descarga un archivo
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="base64String"></param>
        /// <returns></returns>
        async Task saveAsFile(string filename, string base64String)
        {
            await Js.InvokeAsync<object>("saveAsFile", filename, base64String);
        }

        /// <summary>
        /// Rechaza el registro
        /// </summary>
        /// <returns></returns>
        async Task Rechazar()
        {
            await CambiarEstado(15);
        }

        /// <summary>
        /// Autoriza el trámite
        /// </summary>
        /// <returns></returns>
        private async Task Autorizar()
        {
            if (Tramite.TipoTramiteVirtualId == 5)
            {
                if (Files == null || Files.Count == 0)
                {
                    MensajeError = "Por favor seleccione el testamento cerrado";
                    return;
                }

                if (string.IsNullOrEmpty(ClaveTestamento))
                {
                    MensajeError = "Por favor seleccione la clave para el testamento";
                    return;
                }
            }

            if (Coordenadas == null)
            {
                await ConsultarLocalizacion();
                MensajeError = "No se ha obtenido permisos de localización";
                return;
            }

            await CambiarEstado(11);
        }

        private async Task OnFileChangeDocumento(IFileListEntry[] files)
        {
            await OnFileChangeOne(files, 2);
        }

        private async Task OnFileChangeMinuta(IFileListEntry[] files)
        {
            await OnFileChangeOne(files, 3);
        }

        private async Task OnFileChangeMinutaBorrador(IFileListEntry[] files)
        {
            await OnFileChangeOne(files, 6);
        }

        /// <summary>
        /// Carga los datos del archivo en un listado con la info en base64
        /// </summary>
        /// <param name="eventArgs"></param>
        private async Task OnFileChangeOne(IFileListEntry[] files, int type)
        {
            Console.WriteLine($"{type} 🤞🤞🤞");
            if (estadoSeleccionado == 7
                && files.Select(m => new System.IO.FileInfo(m.Name)).Any(m => !m.Extension.Equals(".pdf")))
            {
                MensajeError = "Sólo se debe seleccionar archivos en formato PDF";
                return;
            }

            var existFiles = ExistsFiles(files);
            if (!string.IsNullOrEmpty(existFiles))
            {
                MensajeError = existFiles;
                return;
            }

            LoadingScreenService.Show(string.Empty);

            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    var fileInfo = new System.IO.FileInfo(file.Name);

                    Files.Add(new UploadFileModel
                    {
                        Index = GetIndex(),
                        Nombre = file.Name.Replace(fileInfo.Extension, ""),
                        Formato = file.Type,
                        Data = Convert.ToBase64String(await file.Data.ReadToEndAsync()),
                        Type = type
                    });

                }
            }

            LoadingScreenService.Hide();
        }

        private async Task OnFileChangeDocumentos(IFileListEntry[] files)
        {
            await OnFileChange(files, 2);
        }

        private async Task OnFileChangeMinutas(IFileListEntry[] files)
        {
            await OnFileChange(files, 3);
        }

        private async Task OnFileChangeMinutasBorrador(IFileListEntry[] files)
        {
            await OnFileChange(files, 6);
        }

        private async Task OnFileChangeTestamento(IFileListEntry[] files)
        {
            await OnFileChange(files);
        }

        /// <summary>
        /// Carga los datos de los archivos en un listado con la info en base64
        /// </summary>
        /// <param name="eventArgs"></param>
        private async Task OnFileChange(IFileListEntry[] files, int? type = null)
        {

            MensajeError = string.Empty;

            if (files.Length > 4)
            {
                MensajeError = "Máximo 3 archivos";
                return;
            }

            if (estadoSeleccionado == 7
                && files.Select(m => new System.IO.FileInfo(m.Name)).Any(m => !m.Extension.Equals(".pdf")))
            {
                MensajeError = "Sólo se debe seleccionar archivos en formato PDF";
                return;
            }

            var existFiles = ExistsFiles(files);
            if (!string.IsNullOrEmpty(existFiles))
            {
                MensajeError = existFiles;
                return;
            }

            LoadingScreenService.Show(string.Empty);

            Files.Clear();

            if (files.Length > 0)
            {
                int counter = 0;
                foreach (var file in files)
                {
                    var fileInfo = new System.IO.FileInfo(file.Name);

                    Files.Add(new UploadFileModel
                    {
                        Index = counter,
                        Nombre = file.Name.Replace(fileInfo.Extension, ""),
                        Formato = file.Type,
                        Data = Convert.ToBase64String(await file.Data.ReadToEndAsync()),
                        Type = type
                    });

                    counter++;
                }
            }

            LoadingScreenService.Hide();
        }

        /// <summary>
        /// Obtiene el índice disponible de la lista
        /// </summary>
        /// <returns></returns>
        private int GetIndex()
        {
            int counter = -1;
            do
            {
                counter++;
            }
            while (Files.Exists(x => x.Index == counter));
            return counter;
        }

        private string ExistsFiles(IFileListEntry[] fileListEntry)
        {
            string fileNames = string.Empty;

            fileListEntry.ToList().ForEach(f =>
            {
                var fileInfo = new System.IO.FileInfo(f.Name);
                var fileName = f.Name.Replace(fileInfo.Extension, "");

                if (Files.Exists(x => x.Nombre.Equals(fileName)))
                {
                    fileNames += $" [{f.Name}]";
                }
            });
            return fileNames.Contains("[") ? $"Estos archivos ya se encuentran en la lista: {fileNames}" : null;
        }

        /// <summary>
        /// Recorta el nombre del archivo subido a 30 caracteres
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string ShortName(string value)
        {
            if (value.Length > 30)
            {
                value = value.Substring(0, 30);
                return $"{value} ...";
            }
            return value;
        }

        /// <summary>
        /// Remueve un archivo de la lista
        /// </summary>
        /// <param name="item"></param>
        protected void RemoveFile(UploadFileModel item)
        {
            Files.Remove(item);
            StateHasChanged();
        }

        /// <summary>
        /// Limpia el listado de archivos a enviar
        /// </summary>
        void CleanFiles()
        {
            Files.Clear();
            StateHasChanged();
        }

        /// <summary>
        /// Elimina uno de los archivos
        /// </summary>
        /// <param name="item"></param>
        void RemoveItem(UploadFileModel item)
        {
            Files.RemoveAll(m => m.Index == item.Index);
            StateHasChanged();
        }

        /// <summary>
        /// convierte los datos adicionales a un diccionario
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        Dictionary<string, string> InterpretarJson(string json)
        {
            try
            {
                dynamic data = JsonConvert.DeserializeObject<ExpandoObject>(json);





                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch
            {
                return new Dictionary<string, string>(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Valor", json)
                });
            }
        }

        private async Task<IEnumerable<ActoNotarialModel>> searchActosNotariales(string searchText)
        {
            var datos = ActosNotarialesLista.Where(m => m.Nombre.ToLower().Contains(searchText) || m.Codigo.Contains(searchText)).ToList();
            return await Task.FromResult(datos);
        }

        private int ConvertActoNotarial(ActoNotarialModel acto) => acto.ActoNotarialId;

        private ActoNotarialModel LoadSelectedActo(int? id) => ActosNotarialesLista.FirstOrDefault(p => p.ActoNotarialId == id);

        enum TipoTramiteVirtual
        {
            Matrimonio = 1,
            Autenticacion = 2,
            Compraventa = 3,
            TestamentoAbierto = 4,
            TestamentoCerrado = 5,
            DocumentoPrivado = 6
        }

        enum EstadoTramiteVirtual
        {
            Creado = 1,
            RevisadoCliente = 2,
            PendientePorPagar = 3,
            Pagado = 5,
            MinutaSubida = 7,
            PendienteAutorizar = 9,
            FirmadoNotarioAutorizado = 11,
            Rechazado = 15,
            RevisarCliente = 17
        }

        #endregion
    }
}