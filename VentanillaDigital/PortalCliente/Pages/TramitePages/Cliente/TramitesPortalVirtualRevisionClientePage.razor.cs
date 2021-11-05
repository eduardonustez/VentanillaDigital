using ApiGateway.Contratos.Models.Notario;
using BlazorInputFile;
using HashidsNet;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
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
using System.Security.Claims;
using System.Threading.Tasks;

namespace PortalCliente.Pages.TramitePages.Cliente
{
    public partial class TramitesPortalVirtualRevisionClientePage : ComponentBase
    {
        #region Parameters and Injects
        [Parameter] public string encodedTramiteId { get; set; }

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
        NavigationManager NavManager { get; set; }
        #endregion

        #region Properties And Variables
        public TramiteVirtualModel Tramite { get; set; }
        public List<ArchivoTramiteVirtual> Archivos { get; set; } = new List<ArchivoTramiteVirtual>();
        public List<ActoPorTramiteModel> ActosPorTramite { get; set; } = new List<ActoPorTramiteModel>();
        public long ArchivoSeleccionadoId { get; set; }
        private bool _movil = false;
        public string MensajeError { get; set; }
        public string Observacion { get; set; }
        public List<UploadFileModel> Files { get; set; } = new List<UploadFileModel>();
        public bool IsLoading { get; set; }
        ModalForm ModalIframe { get; set; }
        ModalForm ModalShowPdf { get; set; }
        AuthenticationState context { get; set; }
        public string ClaveTestamento { get; set; }
        public string UrlSubirArchivos { get; set; }
        public Coordenadas Coordenadas { get; set; }
        public string AuthToken { get; set; }
        public TramiteFrm TramiteFrm { get; set; } = new TramiteFrm();
        string RadioValue = "";
        public bool TramiteEnviado { get; set; } = false;
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
        public string Base64File { get; set; }
        public string NameFile { get; set; }
        public string MimeTypeFile { get; set; }
        public long tramiteId;
        private string ClientEmail;
        private int NotariaId;
        private int _clientNumber;

        #endregion

        /// <summary>
        /// Inicio del componenten
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            GetParamsFromUrl();
            ClaimsIdentity identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, ClientEmail),
                new Claim(ClaimTypes.NameIdentifier, ClientEmail),
                new Claim(ClaimTypes.Email, ClientEmail),
                new Claim(ClaimTypes.Role, "Cliente"),
                new Claim("NotariaId", NotariaId.ToString())
                //new Claim("Token", authenticatedUser.Token)
            }, "apiauth_type");

            var user = new ClaimsPrincipal(identity);
            context = new AuthenticationState(user); // await _authenticationStateProvider.GetAuthenticationStateAsync();
            LoadingScreenService.ShowEvent += OnShowLoading;
            LoadingScreenService.HideEvent += OnHideLoading;
            _movil = await DescriptorCliente.EsMovil;
            IsLoading = true;
            AuthToken = await ((CustomAuthenticationStateProvider)_authenticationStateProvider).GetTokenFromAuthenticatedUser();

            await ObtenerActosNotariales();
            await ConsultarTramite(tramiteId);
        }

        void GetParamsFromUrl()
        {
            var hashemail = new Hashids("EMAILCLIENTE", 12, "abcdefghijklmnopqrstuvwxyz0123456789");
            tramiteId = hashemail.DecodeLong(encodedTramiteId)[0];

            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            //if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("em", out var clientEmail))
            //{
            //    ClientEmail = clientEmail; // hashemail.DecodeHex(clientEmail);
            //}
            ClientEmail = "";

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("nt", out var notariaId))
            {
                NotariaId = int.Parse(hashemail.DecodeHex(notariaId));
            }
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("cn", out var clientNumber))
            {
                _clientNumber = int.Parse(hashemail.DecodeHex(clientNumber));
            }
        }

        /// <summary>
        /// Valor de la opcion seleccionada
        /// </summary>
        /// <param name="args"></param>
        void RadioSelection(ChangeEventArgs args)
        {
            RadioValue = args.Value.ToString();
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

            if (!string.IsNullOrEmpty(res.DatosAdicionales))
            {
                dynamic[] datosAdicionales = JsonConvert.DeserializeObject<ExpandoObject[]>(res.DatosAdicionales);
                var personaCorreo = datosAdicionales?.ElementAtOrDefault(_clientNumber)?.Email as string;
                ClientEmail = personaCorreo;
            }

            if (res != null) await CargarDatosTramite(res);
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

            UrlSubirArchivos = string.Format(Configuration["UrlSubirDocumentosMiFirma"], Tramite.TramiteVirtualID);
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

        private async Task OnFileChangeDocumentos(IFileListEntry[] files)
        {
            await OnFileChange(files, 1);
        }

        private async Task OnFileChangeDocumento(IFileListEntry[] files)
        {
            await OnFileChangeOne(files, 1);
        }

        /// <summary>
        /// Carga los datos del archivo en un listado con la info en base64
        /// </summary>
        /// <param name="eventArgs"></param>
        private async Task OnFileChangeOne(IFileListEntry[] files, int type)
        {
            //if (estadoSeleccionado == 7
            //    && files.Select(m => new System.IO.FileInfo(m.Name)).Any(m => !m.Extension.Equals(".pdf")))
            //{
            //    MensajeError = "Sólo se debe seleccionar archivos en formato PDF";
            //    return;
            //}

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

            //if (estadoSeleccionado == 7
            //    && files.Select(m => new System.IO.FileInfo(m.Name)).Any(m => !m.Extension.Equals(".pdf")))
            //{
            //    MensajeError = "Sólo se debe seleccionar archivos en formato PDF";
            //    return;
            //}

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
        /// Cambia de estado un trámite
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        async Task CambiarEstado()
        {
            try
            {
                MensajeError = string.Empty;
                var res = await tramitesVirtualService.CambiarEstadoCliente(
                    tramiteId,
                    Files,
                    RadioValue.Equals("Acuerdo"),
                    Observacion,
                    ClientEmail
                );

                if (res != null)
                {
                    ShowSuccessNotification("Envío exitoso", "El trámite ha enviado exitosamente.");

                    TramiteEnviado = true;
                    //await ConsultarTramite(tramiteId);
                    //StateHasChanged();
                }
            }
            catch (Exception ex)
            {
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
        /// Mensaje de notificación
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        void ShowSuccessNotification(string title, string text)
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = title, Detail = text, Duration = 4000 };
            notificationService.Notify(message);
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
        #endregion
    }
}